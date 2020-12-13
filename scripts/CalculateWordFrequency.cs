using System.Runtime;
using System.Data;
using System.Data.Common;
using System;
using DmcSocial.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using DmcSocial.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;

namespace scripts
{
  public class WordFrequencyCalculator
  {
    public PostRepository GetRepository(DbContextOptionsBuilder<AppDbContext> optionsBuilder)
    {
      var db = new AppDbContext(optionsBuilder.Options);
      db.ChangeTracker.AutoDetectChangesEnabled = false;
      return new PostRepository(db);
    }
    public void Run(string[] args)
    {
      var dbURL = args[0];
      var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
      optionsBuilder.UseNpgsql(dbURL);
      var offset = 0;
      var limit = 100;
      double ms = 0;
      var total = 0;
      do
      {
        var startTime = DateTime.Now;
        var posts = GetRepository(optionsBuilder).GetPosts(new List<string>(), new DmcSocial.Models.PostListParams(offset, limit, null, null)).Result;
        var postIds = posts.Select(u => u.Id);
        var wordFreq = new List<WordFrequency>();
        foreach (var post in posts)
        {
          var postWordFreq = GetRepository(optionsBuilder).GetWordFrequencies(post).Result;
          wordFreq.AddRange(postWordFreq);
        }
        using (var db = new AppDbContext(optionsBuilder.Options))
        {
          db.RemoveRange(db.WordFrequencies.Where(w => postIds.Contains(w.PostId)));
          db.SaveChanges();
        }
        using (var db = new AppDbContext(optionsBuilder.Options))
        {
          db.AddRange(wordFreq);
          db.SaveChanges();
        }
        offset += posts.Count;
        var elapse = DateTime.Now - startTime;
        ms += elapse.TotalMilliseconds;
        total += posts.Count;
        Console.WriteLine($"{elapse.TotalMilliseconds}ms | Calculate words of {posts.Count} post(s): {string.Join(",", posts.Select(u => $"#{u.Id}"))}");
        if (posts.Count == 0) break;
      } while (true);
      Console.WriteLine($"Calculated word frequency of {total} post(s), take {ms} ms");
    }
  }
}