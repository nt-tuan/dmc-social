using System.Collections.Immutable;
using System;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Blogs.Repositories;
using System.Linq;
using System.Text.RegularExpressions;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.RankingWorker.Jobs;

namespace Console.Jobs
{
  public class FixMediumPostJob : Job
  {
    public override void Run()
    {
      int total = 0, count;
      do
      {
        count = ChuckFix(total);
        total += count;
        System.Console.WriteLine($"Updated {total}");
      } while (count > 0);
    }
    public bool FixPostNewLine(Post post)
    {
      post.Subtitle = post.Subtitle.Replace("  ", "<br />");
      post.Content = post.Content.Replace("  ", "<br />");
      return true;
    }
    public int ChuckFix(int offset)
    {
      var db = GetDbContext();
      var posts = db.Posts.Skip(offset).Take(100).OrderBy(u => u.Id).ToList();
      foreach (var post in posts)
      {
        if (FixPostNewLine(post))
        {
          db.Entry(post).Property(u => u.Content).IsModified = true;
          db.Entry(post).Property(u => u.Subtitle).IsModified = true;
        }
      }
      db.SaveChanges();
      return posts.Count();
    }
  }
}