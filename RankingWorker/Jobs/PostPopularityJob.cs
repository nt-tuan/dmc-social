using System;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class PostPopularityJob : Job
  {
    readonly int CommentWeight;
    readonly int ViewWeight;
    readonly int InteractiveWeight;
    readonly int RngWeight;
    public PostPopularityJob()
    {
      CommentWeight = 10;
      ViewWeight = 1;
      InteractiveWeight = 10;
      RngWeight = 12;
    }
    public decimal GetPopularity(int rank, int total)
    {
      return rank / (decimal)total * InteractiveWeight +
      (decimal)(new Random().NextDouble() * RngWeight);
    }
    public override void Run()
    {
      Console.WriteLine($"Start calculating post popularity at {DateTimeOffset.Now}");
      var db = GetDbContext();
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
        InteractiveScore = post.CommentCount * CommentWeight + post.ViewCount * ViewWeight,
        Rank = 0.0
      }).OrderBy(post => post.InteractiveScore).ToList();
      var newPosts = new List<Post>();
      for (var i = 0; i < orderedPosts.Count; i++)
      {
        var post = orderedPosts[i].Post;
        post.Popularity = GetPopularity(i, orderedPosts.Count);
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