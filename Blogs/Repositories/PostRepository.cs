using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.ExceptionServices;
using System.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Entities;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Blogs.Interfaces;
using System.Collections.Immutable;
using System.IO;
using Microsoft.EntityFrameworkCore.Query;
using PostgreSQLCopyHelper;
using Npgsql;
using Microsoft.EntityFrameworkCore.Internal;

namespace ThanhTuan.Blogs.Repositories
{
  public class PostRepository : IPostRepository
  {
    private readonly AppDbContext _db;
    private readonly IRepository _repo;
    public PostRepository(AppDbContext db)
    {
      _db = db;
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
      var tagsCount = tags?.Count();
      if (tagsCount != null && tagsCount > 0)
      {
        query = query
        .Where(u => u.DateRemoved == null)
        .Where(u => u.PostTags.Where(u => tags.Contains(u.TagId)).Count() >= tagsCount);
      }
      return query;
    }

    public async Task<List<Post>> GetPosts(PostListParameter parameter)
    {
      var query = GetPostsByTagsQuery(parameter.Tags);
      query = Helper.ApplyListParam(query, parameter);
      var posts = await query.ToListAsync();
      return posts;
    }

    public async Task<int> CountPosts(PostCountParameter parameter)
    {
      var query = GetPostsByTagsQuery(parameter.Tags);
      query = Helper.ApplyFilters(query, parameter.Filter);
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
      var now = DateTimeOffset.Now;
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
      var entity = await _repo.GetQuery<Tag>().FirstOrDefaultAsync(e => e.Slug == tag);
      return entity;
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

    public async Task<List<Post>> SearchPosts(List<string> tagIds, List<string> keywords, int offset, int limit)
    {
      keywords = keywords?.Select(w => Helper.NormalizeString(w)).ToList();
      var query = _db.PostTags.
        Where(u =>
          tagIds == null || tagIds.Count == 0 ||
          tagIds.Contains(u.TagId)).
        GroupBy(u => u.PostId).
        Select(group => new
        {
          PostId = group.Key,
          Rank = group.LongCount()
        });
      if (keywords != null && keywords.Count > 0)
      {
        var wordFrequencyQuery = _db.WordFrequencies.
        Where(w => keywords.Contains(w.Word)).
        GroupBy(u => u.PostId).
        Select(u => new
        {
          PostId = u.Key,
          Frequency = u.Sum(w => w.Frequency),
          MatchedWordCount = u.Count()
        });
        query = query.Join(
          wordFrequencyQuery,
          i => i.PostId,
          o => o.PostId,
          (o, i) => new
          {
            o.PostId,
            Rank = o.Rank * 10000 + i.MatchedWordCount * 100 + i.Frequency / i.MatchedWordCount,
          });
      }
      var posts = await query.
      OrderByDescending(u => u.Rank).
      Skip(offset).Take(limit).
      Join(_repo.GetQuery<Post>(), o => o.PostId, i => i.Id, (o, i) => i).
      ToListAsync();
      return posts;
    }

    public async Task<int> CountSearchedPosts(List<string> tagIds, List<string> keywords)
    {
      keywords = keywords.Select(w => Helper.NormalizeString(w)).ToList();
      var count = await _db.WordFrequencies.
      Where(w => keywords.Contains(w.Word)).
      GroupBy(u => u.PostId).
      Select(u => new { PostId = u.Key }).
      Join(_db.Posts.
        Where(u =>
          tagIds.Count == 0 ||
          u.PostTags.Any(u => tagIds.Contains(u.TagId))),
          o => o.PostId,
          i => i.Id,
          (o, i) => new { Post = i }).
      CountAsync();
      return count;
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
      _db.Entry(entity).Property(post => post.PostAccessUsers).IsModified = true;
      _db.Entry(entity).Property(post => post.PostRestrictionType).IsModified = true;
      _db.Entry(entity).Property(post => post.CanComment).IsModified = true;

      _repo.Update(entity, entity.CreatedBy);
      await _db.SaveChangesAsync();
      return entity;
    }

  }
}
