using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class RankingJob : Job
  {
    public DateTimeOffset GetLastRunAt()
    {
      using var db = GetDbContext();
      if (!db.IndexingSchedules.Where(u => u.State == IndexingSchedule.IndexingSuccess).Any())
      {
        return DateTimeOffset.MinValue;
      }
      var startAt = db.IndexingSchedules.Where(u => u.State == IndexingSchedule.IndexingSuccess).Max(u => u.EndAt);
      return startAt;
    }
    public List<Post> GetPosts(DateTimeOffset startAt, DateTimeOffset endAt, int? lastId)
    {
      using var db = GetDbContext();
      var posts = db.Posts.Where(u => u.DateRemoved == null &&
      u.LastModifiedTime > startAt &&
      u.LastModifiedTime <= endAt &&
      u.Id > (lastId ?? 0)
      ).
      OrderBy(u => u.Id).
      Take(10).
      ToList();
      return posts;
    }
    int? ChuckCalculateWorkFrequency(DateTimeOffset startAt, DateTimeOffset endAt, int? lastId)
    {
      var watch = new Stopwatch();
      watch.Start();
      var newPosts = GetPosts(startAt, endAt, lastId);
      var postIds = newPosts.Select(u => u.Id);
      Console.Write($"Calculate word frequency of {postIds.Count()} ");
      if (postIds.Count() > 0)
      {
        var maxId = postIds.Max();
        var minId = postIds.Min();
        Console.WriteLine($"posts: {string.Join(", ", postIds.Select(u => $"#{u}"))}");
      }
      else
      {
        Console.WriteLine();
      }
      var newPostWfs = new List<WordFrequency>();
      foreach (var post in newPosts)
      {
        var wf = RankingUtilities.GetWordFrequencies(post);
        newPostWfs.AddRange(wf);
      }
      using (var db = GetDbContext())
      {
        db.WordFrequencies.
            Where(w => postIds.Contains(w.PostId)).Delete();
      }
      RankingUtilities.BulkInsertWordFrequencies(newPostWfs, GetDatabaseURL());
      watch.Stop();
      Console.WriteLine($"Done in {watch.ElapsedMilliseconds} ms");
      if (postIds.Count() == 0) return null;
      return postIds.Max();
    }
    public void CalculateWorkFrequency(DateTimeOffset startAt, DateTimeOffset endAt)
    {
      int? id = null;
      do
      {
        id = ChuckCalculateWorkFrequency(startAt, endAt, id);
      } while (id != null);
    }
    public void RemoveDeletedPost(DateTimeOffset startAt, DateTimeOffset endAt)
    {
      using (var db = GetDbContext())
      {
        var postIds = db.Posts.Where(u => u.DateRemoved != null &&
          u.DateRemoved >= startAt &&
          u.DateRemoved <= endAt).Select(u => u.Id).ToList();
        db.WordFrequencies.Where(u => postIds.Contains(u.PostId)).Delete();
      }
    }
    public IndexingSchedule NewJobRecord(DateTimeOffset startAt, DateTimeOffset endAt)
    {
      var record = new IndexingSchedule
      {
        StartAt = startAt,
        RunAt = DateTimeOffset.Now,
        EndAt = endAt,
        State = IndexingSchedule.IndexingPending
      };
      var db = GetDbContext();
      db.IndexingSchedules.Add(record);
      db.SaveChanges();
      return record;
    }
    public void UpdateJobRecord(IndexingSchedule record)
    {
      var db = GetDbContext();
      db.Update(record);
      db.SaveChanges();
    }
    public override void Run()
    {
      var startAt = GetLastRunAt();
      var endAt = DateTimeOffset.Now;
      var record = NewJobRecord(startAt, endAt);
      try
      {
        CalculateWorkFrequency(startAt, endAt);
        RemoveDeletedPost(startAt, endAt);
        new PostPopularityJob().Run();
        new TagPopularityJob().Run();
        new TagCorrellationCoefficientJob().Run();
        record.State = IndexingSchedule.IndexingSuccess;
      }
      catch (Exception e)
      {
        record.Message = e.Message + "\n" + e.StackTrace;
        Console.WriteLine(record.Message);
        record.State = IndexingSchedule.IndexingFailed;
        throw e;
      }
      finally
      {
        UpdateJobRecord(record);
      }
    }
  }
}


