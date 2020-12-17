using System;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;
using DmcSocial.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial
{
  public class CalculatePostPopularity
  {
    readonly DbContextOptionsBuilder<AppDbContext> _dbOptions;
    public CalculatePostPopularity(string dbURL)
    {
      _dbOptions = new DbContextOptionsBuilder<AppDbContext>();
      _dbOptions.UseNpgsql(dbURL);
    }

    public void Run()
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
        Popularity = post.CommentCount * 4 + post.ViewCount,
        Rank = 0.0
      }).OrderByDescending(post => post.Popularity).ToList();
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
    }
  }
}