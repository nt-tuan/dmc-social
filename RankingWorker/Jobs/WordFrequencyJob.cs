using System.Runtime;
using System.Data;
using System.Data.Common;
using System;
using ThanhTuan.Blogs.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using ThanhTuan.Blogs;
using Z.EntityFramework.Plus;
using PostgreSQLCopyHelper;
using Npgsql;
using System.Threading;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class WordFrequencyJob : Job
  {
    readonly int? _startID;
    readonly int? _endID;
    public WordFrequencyJob()
    {
      var startID = Environment.GetEnvironmentVariable("START_ID");
      if (!string.IsNullOrEmpty(startID))
      {
        _startID = int.Parse(startID);
      }
      var endID = Environment.GetEnvironmentVariable("END_ID");
      if (!string.IsNullOrEmpty(endID))
      {
        _endID = int.Parse(endID);
      }
    }
    public PostRepository GetRepository()
    {
      var db = GetDbContext();
      db.ChangeTracker.AutoDetectChangesEnabled = false;
      return new PostRepository(db);
    }

    int GetChuckSize()
    {
      var defaultChuckSize = 100;
      var chuckSize = Environment.GetEnvironmentVariable("CHUCK_SIZE");
      if (!string.IsNullOrEmpty(chuckSize))
      {
        return int.Parse(chuckSize);
      }
      return defaultChuckSize;
    }
    public override void Run()
    {
      var chuckSize = GetChuckSize();
      double ms = 0;
      var db = GetDbContext();
      int maxId, id;
      if (_startID != null)
      {
        maxId = _startID.Value;
      }
      else
      {
        maxId = db.Posts.Max(u => u.Id);
      }
      if (_endID != null)
      {
        id = _endID.Value;
      }
      else
      {
        id = db.Posts.Min(u => u.Id);
      }
      // var id = 38031;
      Console.WriteLine($"Start calculate word frequency with ID range {id}:{maxId}| ");
      while (id <= maxId)
      {
        var startTime = DateTimeOffset.Now;
        var next = TryRunChuck(id, id + chuckSize - 1, 0, null);
        ms += (DateTimeOffset.Now - startTime).TotalMilliseconds;
        id += chuckSize;
      }
      Console.WriteLine($"Calculated word frequencies, take {ms} ms");
    }

    bool TryRunChuck(int startID, int endID, int times, Exception lastException)
    {
      if (times > 5 && lastException != null)
      {
        throw lastException;
      }
      if (times > 5 && lastException == null) throw new Exception("Unknown error");
      try
      {
        return RunChuck(startID, endID);
      }
      catch (Exception e)
      {
        var sleepMs = Math.Max(1, times) * 2000;
        Console.WriteLine($"Get error: {e.Message}, retry in {sleepMs} ms");
        Thread.Sleep(Math.Max(1, times) * 1000);
        return TryRunChuck(startID, endID, times + 1, e);
      }
    }
    bool RunChuck(int startID, int endID)
    {
      Console.Write($"Run chuck {startID}:{endID}| ");
      var startTime = DateTimeOffset.Now;
      List<Post> posts;
      using (var db = GetDbContext())
      {
        posts = db.Posts.Where(u => u.DateRemoved == null && u.Id >= startID && u.Id <= endID).ToList();
      }
      if (posts.Count == 0) return false;
      var postIds = posts.Select(u => u.Id);
      var wordFreq = new List<WordFrequency>();
      foreach (var post in posts)
      {
        var postWordFreq = RankingUtilities.GetWordFrequencies(post);
        wordFreq.AddRange(postWordFreq);
      }
      var calculateTime = DateTimeOffset.Now - startTime;
      Console.Write($"Calculate time: {calculateTime.TotalMilliseconds}, ");
      using (var db = GetDbContext())
      {
        // db.RemoveRange(db.WordFrequencies.
        // Where(w => postIds.Contains(w.PostId)));
        var startDeleteTime = DateTimeOffset.Now;
        db.WordFrequencies.
        Where(w => w.PostId >= startID && w.PostId <= endID).Delete();
        Console.Write($"Delete time: {(DateTimeOffset.Now - startDeleteTime).TotalMilliseconds} ");
        var startInsertTime = DateTimeOffset.Now;
        RankingUtilities.BulkInsertWordFrequencies(wordFreq, GetDatabaseURL());
        Console.Write($"Insert time: {(DateTimeOffset.Now - startInsertTime).TotalMilliseconds} ");
        // db.AddRange(wordFreq);
        // db.SaveChanges();
      }
      var elapse = DateTimeOffset.Now - startTime;
      Console.WriteLine($" | Calculate words of {posts.Count} post(s) from #{startID} to ${endID}, found {wordFreq.Count} words");
      return true;
    }
  }
}