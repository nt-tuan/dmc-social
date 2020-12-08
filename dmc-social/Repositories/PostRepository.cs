using System.ComponentModel;
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
namespace DmcSocial.Repositories
{
  public class PostRepository : IPostRepository
  {
    private readonly AppDbContext _db;
    public PostRepository(AppDbContext db)
    {
      _db = db;
    }

    private IQueryable<Post> GetPostQuery()
    {
      return _db.Posts
      .Include(u => u.PostTags).Where(post => post.DateRemoved == null).AsQueryable();
    }

    public async Task<Post> CreatePost(Post post, string actor)
    {
      var now = DateTime.Now;
      post.CreatedBy = actor;
      post.LastModifiedBy = actor;
      post.DateCreated = now;
      post.LastModifiedTime = now;
      post.PostTags = null;
      _db.Add(post);
      await _db.SaveChangesAsync();
      return post;
    }

    public async Task<Post> GetPostById(int id, bool load = true)
    {
      var query = _db.Posts.AsQueryable();
      if (load)
      {
        query = GetPostQuery();
      }
      var post = await query.Where(post => post.Id == id && post.DateRemoved == null).FirstOrDefaultAsync();
      return post;
    }

    public async Task<PostMetric> GetPostMetricById(int id)
    {
      var post = await _db.Posts.FindAsync(id);
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
      entity.LastModifiedTime = now;
      entity.LastModifiedBy = actor;
      entity.CoverImageURL = coverImageURL;
      entity.Subtitle = subtitle;
      foreach (var postTag in entity.PostTags)
      {
        postTag.Tag.LastModifiedTime = now;
      }
      _db.Update(entity);
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
      entity.DateRemoved = DateTime.Now;
      foreach (var postTag in entity.PostTags)
      {
        postTag.Tag.PostCount--;
      }
      _db.Update(entity);
      await _db.SaveChangesAsync();
    }

    private async Task<Tag> GetTag(string tag)
    {
      var entity = await _db.Tags.FindAsync(tag);
      return entity;
    }
    private void SetTagModifiedTime(Post post, Tag tag)
    {
      if (post.LastModifiedTime == null)
        return;
      if (tag.LastModifiedTime == null)
      {
        tag.LastModifiedTime = post.LastModifiedTime;
        return;
      }
      if (tag.LastModifiedTime > post.LastModifiedTime)
        return;
      tag.LastModifiedTime = post.LastModifiedTime;
    }
    public async Task AddTag(Post post, string tag)
    {
      var tagEntity = await GetTag(tag);
      if (tagEntity == null)
        throw TagException.TagNotFound;

      var entity = new PostTag { PostId = post.Id, TagId = tag };
      _db.PostTags.Add(entity);
      tagEntity.PostCount++;
      SetTagModifiedTime(post, tagEntity);
      _db.Tags.Update(tagEntity);
      await _db.SaveChangesAsync();
    }

    public async Task RemoveTag(Post post, string tag)
    {
      var entity = await _db.PostTags.FindAsync(post.Id, tag);
      var tagEntity = await _db.Tags.FindAsync(tag);
      if (entity == null)
        throw TagException.TagNotFound;
      _db.PostTags.Remove(entity);
      tagEntity.PostCount--;
      await _db.SaveChangesAsync();
    }

    public async Task<List<Post>> SearchPosts(List<string> tagIds, GetListParams<Post> param)
    {
      if (tagIds.Count() == 0)
        return new List<Post>();
      var relatedPostTags = await _db.PostTags
      .Where(u => tagIds.Contains(u.TagId) && u.Post.DateRemoved == null)
      .GroupBy(u => u.PostId)
      .Select(u => new
      {
        PostId = u.Key,
        Count = u.Count()
      })
      .Skip(param.GetSkipNumber())
      .Take(param.Limit)
      .OrderByDescending(u => u.Count)
      .ToListAsync();

      var postIds = relatedPostTags.Select(u => u.PostId);
      if (postIds.Count() == 0)
      {
        return new List<Post>();
      }
      var posts = await _db.Posts.Where(u => postIds.Contains(u.Id)).ToListAsync();

      posts.Sort((a, b) =>
      {
        var countA = postIds.FirstOrDefault(u => u == a.Id);
        var countB = postIds.FirstOrDefault(u => u == b.Id);
        return countA.CompareTo(countB);
      });
      return posts;
    }

    public async Task<int> IncreaseView(int postId)
    {
      var count = await _db.Posts.Where(post => post.Id == postId).Select(post => new { post.ViewCount }).FirstAsync();
      var newCount = count.ViewCount + 1;
      _db.Posts.Attach(new Post { Id = postId, ViewCount = newCount }).Property(post => post.ViewCount).IsModified = true;
      await _db.SaveChangesAsync();
      return newCount;
    }

    public async Task<Post> UpdatePostConfig(int postId, int postRestrictionType, string[] accessUsers)
    {
      var entity = await _db.Posts.FindAsync(postId);
      entity.PostAccessUsers = accessUsers;
      entity.PostRestrictionType = postRestrictionType;
      _db.Posts.Attach(entity).Property(post => new { post.PostAccessUsers, post.PostRestrictionType }).IsModified = true;
      await _db.SaveChangesAsync();
      return entity;
    }
  }
}
