﻿using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.ExceptionServices;
using System.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Models;
using Microsoft.EntityFrameworkCore;
using DmcSocial.Interfaces;
using System.Collections.Immutable;
using System.IO;

namespace DmcSocial.Repositories
{
  public class PostRepository : IPostRepository
  {
    private readonly AppDbContext _db;
    private readonly int _titleWeight;
    private readonly IRepository _repo;
    public PostRepository(AppDbContext db)
    {
      _db = db;
      _titleWeight = 20;
      _repo = new Repository(db);
    }

    private IQueryable<Post> GetPostQuery()
    {
      return _repo.GetQuery<Post>().Include(u => u.PostTags);
    }

    public async Task<Post> CreatePost(Post post, string actor)
    {
      post.PostTags = null;
      _repo.Add(post, actor);
      await _db.SaveChangesAsync();
      return post;
    }

    public async Task<Post> GetPostById(int id, bool load = true)
    {
      var query = _repo.GetQuery<Post>();
      if (load)
      {
        query = GetPostQuery();
      }
      var post = await query.FirstOrDefaultAsync(post => post.Id == id);
      return post;
    }

    public async Task<PostMetric> GetPostMetricById(int id)
    {
      var post = await _repo.GetQuery<Post>().FirstOrDefaultAsync(u => u.Id == id);
      return new PostMetric(post);
    }

    private IQueryable<Post> GetPostsByTagsQuery(List<string> tags)
    {
      var query = GetPostQuery();
      var tagsCount = tags.Count();
      if (tags.Count > 0)
      {
        query = query
        .Where(u => u.DateRemoved == null)
        .Where(u => u.PostTags.Where(u => tags.Contains(u.TagId)).Count() >= tagsCount);
      }
      return query;
    }

    public async Task<List<Post>> GetPosts(List<string> tags, GetListParams<Post> paging)
    {
      var query = GetPostsByTagsQuery(tags);
      query = Helper.ApplyPaging(query, paging);
      var posts = await query.ToListAsync();
      return posts;
    }

    public async Task<int> CountPosts(List<string> tags)
    {
      var query = GetPostsByTagsQuery(tags);
      var count = await query.CountAsync();
      return count;
    }

    public async Task<Post> UpdatePostContent(int postId, string title, string subtitle, string coverImageURL, string content, string actor)
    {
      var entity = await _db.Posts
      .Include(u => u.PostTags)
      .ThenInclude(pt => pt.Tag)
      .FirstOrDefaultAsync(post => post.Id == postId);
      if (entity == null)
        throw PostException.PostNotFound;
      var now = DateTime.Now;
      // Ignore update posttags
      entity.Title = title;
      entity.Content = content;
      entity.CoverImageURL = coverImageURL;
      entity.Subtitle = subtitle;
      foreach (var postTag in entity.PostTags)
      {
        postTag.Tag.LastModifiedTime = now;
      }
      _db.Update(entity);
      _repo.Update(entity, actor);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task DeletePost(int id, string actor)
    {
      var entity = await _db.Posts
      .Include(u => u.PostTags)
      .ThenInclude(pt => pt.Tag)
      .Where(post => post.Id == id && post.DateRemoved == null).FirstOrDefaultAsync();
      if (entity == null || entity.DateRemoved != null)
        throw PostException.PostNotFound;
      foreach (var postTag in entity.PostTags)
      {
        postTag.Tag.PostCount--;
      }
      _db.UpdateRange(entity.PostTags);
      _repo.Delete(entity, actor);
      await _db.SaveChangesAsync();
    }

    private async Task<Tag> GetTag(string tag)
    {
      var entity = await _repo.GetQuery<Tag>().FirstOrDefaultAsync(e => e.NormalizeValue == tag);
      return entity;
    }
    private void SetTagModifiedTime(Post post, Tag tag)
    {
      if (tag.LastModifiedTime > post.LastModifiedTime)
        return;
      tag.LastModifiedTime = post.LastModifiedTime;
    }
    public async Task AddTag(Post post, string tag, string actor)
    {
      var existed = await _db.PostTags.AnyAsync(u => u.PostId == post.Id && u.TagId == tag);
      if (existed) return;
      var tagEntity = await GetTag(tag);
      if (tagEntity == null)
        throw TagException.TagNotFound;
      var entity = new PostTag(actor) { PostId = post.Id, TagId = tag };
      _repo.Add(entity, actor);

      tagEntity.PostCount++;
      _db.Tags.Attach(tagEntity).Property(u => u.PostCount).IsModified = true;

      await _db.SaveChangesAsync();
    }

    public async Task RemoveTag(Post post, string tag, string actor)
    {
      var entity = await _db.PostTags.
      Include(u => u.Tag).
      FirstOrDefaultAsync(u => u.PostId == post.Id && u.TagId == tag);
      if (entity == null)
        throw TagException.TagNotFound;
      _repo.Delete(entity, actor);

      entity.Tag.PostCount--;
      _db.Tags.Attach(entity.Tag).Property(u => u.PostCount).IsModified = true;
      await _db.SaveChangesAsync();
    }

    public async Task<List<Post>> SearchPosts(List<string> tagIds, List<string> keywords, GetListParams<Post> param)
    {
      tagIds.Sort();
      var tagsLike = string.Join("%", tagIds.Select(tag => Helper.NormalizeTag(tag)));
      keywords = keywords.Select(w => Helper.NormalizeString(w)).ToList();
      var result = await _db.WordFrequencies.
      Where(w => keywords.Contains(w.Word)).
      GroupBy(u => u.PostId).
      Select(u => new
      {
        PostId = u.Key,
        Frequency = u.Sum(w => w.Frequency),
        MatchedWordCount = u.Count()
      }).
      Skip(param.Offset).Take(param.Limit).
      OrderByDescending(u => new { u.MatchedWordCount, u.Frequency }).ToListAsync();

      var postIds = result.Select(u => u.PostId);
      var posts = await _db.Posts.Where(u => postIds.Contains(u.Id)).ToListAsync();
      var postDict = posts.ToDictionary(u => u.Id);
      var postsResult = result.Select(u => postDict.ContainsKey(u.PostId) ? postDict[u.PostId] : null).Where(u => u != null);
      return postsResult.ToList();
    }

    public async Task<int> CountSearchedPosts(List<string> tagIds, List<string> keywords)
    {
      var tagsLike = string.Join("%", tagIds.Select(tag => Helper.NormalizeTag(tag)));
      keywords = keywords.Select(w => Helper.NormalizeString(w)).ToList();
      var result = await _db.WordFrequencies.
      Where(w => keywords.Contains(w.Word)).
      GroupBy(u => u.PostId).CountAsync();
      return result;
    }

    public async Task<int> IncreaseView(int postId)
    {
      var count = await _db.Posts.Where(post => post.Id == postId).Select(post => new { post.ViewCount }).FirstAsync();
      var newCount = count.ViewCount + 1;
      _db.Posts.Attach(new Post { Id = postId, ViewCount = newCount }).Property(post => post.ViewCount).IsModified = true;
      await _db.SaveChangesAsync();
      return newCount;
    }

    public async Task<Post> UpdatePostConfig(int postId, int postRestrictionType, string[] accessUsers, bool canComment)
    {
      var entity = await _db.Posts.FindAsync(postId);
      entity.PostAccessUsers = accessUsers;
      entity.PostRestrictionType = postRestrictionType;
      entity.CanComment = canComment;
      _db.Posts.Attach(entity).Property(post => new
      {
        post.PostAccessUsers,
        post.PostRestrictionType,
        post.CanComment
      }).IsModified = true;
      _repo.Update(entity, entity.CreatedBy);
      await _db.SaveChangesAsync();
      return entity;
    }

    public Dictionary<string, long> ExtractWordFrequency(string text)
    {
      var words = Helper.NormalizeString(text).Split("_");
      var dict = new Dictionary<string, long>();
      foreach (var word in words)
      {
        if (word.Length < 2) continue;
        if (dict.ContainsKey(word))
        {
          dict[word]++;
        }
        else
        {
          dict[word] = 1;
        }
      }
      return dict;
    }

    public List<WordFrequency> GetWordFrequencies(Post post)
    {
      var dict = ExtractWordFrequency(post.Content);
      var titleDict = ExtractWordFrequency(post.Title);
      foreach (var pair in titleDict)
      {
        var value = pair.Value * _titleWeight;
        if (dict.ContainsKey(pair.Key))
        {
          dict[pair.Key] += value;
        }
        else
        {
          dict[pair.Key] = value;
        }
      }
      return dict.Select(pair =>
      {
        return new WordFrequency
        {
          Word = pair.Key,
          Frequency = pair.Value,
          PostId = post.Id
        };
      }).ToList();
    }

    public async Task UpdateWordFrequencies(Post post)
    {
      var wordFrequencies = GetWordFrequencies(post);
      var outdated = await _db.WordFrequencies.Where(u => u.PostId == post.Id).ToListAsync();
      _db.ChangeTracker.AutoDetectChangesEnabled = false;
      _db.RemoveRange(outdated);
      await _db.SaveChangesAsync();
      _db.AddRange(wordFrequencies);
      await _db.SaveChangesAsync();
    }
  }
}
