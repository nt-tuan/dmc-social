using System;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;
using DmcSocial.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial
{
  public class CalculateTagPopularity
  {
    readonly DbContextOptionsBuilder<AppDbContext> _dbOptions;
    public CalculateTagPopularity(string dbURL)
    {
      _dbOptions = new DbContextOptionsBuilder<AppDbContext>();
      _dbOptions.UseNpgsql(dbURL);
    }

    List<Post> CalculatePostPopularity()
    {
      var db = new AppDbContext(_dbOptions.Options);
      var posts = db.Posts.AsNoTracking().Where(u => u.DateRemoved == null).
      Select(u => new Post
      {
        Id = u.Id,
        CommentCount = u.CommentCount,
        ViewCount = u.ViewCount
      }).ToList();
      var orderedPosts = posts.Select(post => new
      {
        Post = post,
        InteractiveScore = post.CommentCount * 4 + post.ViewCount,
        Rank = 0.0
      }).OrderByDescending(post => post.InteractiveScore).ToList();
      var newPosts = new List<Post>();
      for (var i = 0; i < orderedPosts.Count; i++)
      {
        var post = orderedPosts[i].Post;
        post.Popularity = ((orderedPosts.Count - i) / (decimal)orderedPosts.Count * 10) + (decimal)(new Random().NextDouble() * 12);
        newPosts.Add(post);
      }
      foreach (var item in newPosts)
      {
        db.Entry(item).Property(u => u.Popularity).IsModified = true;
      }
      db.SaveChanges();
      return newPosts;
    }
    class TagCoefficient
    {
      public string Tag { get; set; }
      public decimal Popularity { get; set; }
    }
    List<TagCoefficient> GetTagCorrelationCoefficients()
    {
      return new AppDbContext(_dbOptions.Options).TagCorrelationCoefficients.
      GroupBy(u => u.TagX).
      Select(u => new TagCoefficient
      {
        Tag = u.Key,
        Popularity = (1 - u.Average(v => v.Coefficient)) * u.Count() * 10
      }).ToList();
    }
    List<PostTag> GetPostTags()
    {
      return new AppDbContext(_dbOptions.Options).PostTags.Where(u => u.DateRemoved == null).ToList();
    }

    public void Run()
    {
      var posts = CalculatePostPopularity();
      var postDictionary = posts.ToDictionary(u => u.Id);
      var coefficients = GetTagCorrelationCoefficients().ToDictionary(u => u.Tag);
      var tagsCount = GetPostTags().GroupBy(u => u.TagId).Select(u => new { u.Key, Count = u.Count() }).ToDictionary(u => u.Key);
      var maxCount = tagsCount.Select(u => u.Value.Count).Max();
      var maxPopularity = coefficients.Select(u => u.Value.Popularity).Max();
      var updatedTags = new List<Tag>();
      var db = new AppDbContext(_dbOptions.Options);
      foreach (var item in coefficients)
      {
        var tag = new Tag
        {
          Slug = item.Key,
          Popularity = item.Value.Popularity / maxPopularity * 10
          + tagsCount[item.Key].Count / maxCount * 10
          + (decimal)(new Random().NextDouble() * 10)
        };
        db.Attach(tag).Property(u => u.Popularity).IsModified = true;
      }
      db.SaveChanges();
    }
  }
}