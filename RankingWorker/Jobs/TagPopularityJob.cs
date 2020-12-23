using System;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Migrations;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class TagPopularityJob : Job
  {
    readonly int _postNumberRankWeight;
    readonly int _tagPopularityWeight;
    readonly int _rngWeight;
    public TagPopularityJob()
    {
      _postNumberRankWeight = 20;
      _rngWeight = 10;
      _tagPopularityWeight = 10;
    }
    class TagCoefficient
    {
      public string Tag { get; set; }
      public decimal Popularity { get; set; }
    }
    List<TagCoefficient> GetTagCorrelationCoefficients()
    {
      return GetDbContext().TagCorrelationCoefficients.
      GroupBy(u => u.TagY).
      Select(u => new TagCoefficient
      {
        Tag = u.Key,
        Popularity = u.Average(v => v.Coefficient) * u.Count()
      }).ToList();
    }
    public Dictionary<string, decimal> GetTagRankDictionary()
    {
      var db = GetDbContext();
      var tags = db.PostTags.Where(u => u.DateRemoved == null).
      Join(db.Posts, o => o.PostId, i => i.Id,
      (o, i) => new { o.TagId, o.PostId, i.Popularity }).ToList();
      var tagGroups = tags.GroupBy(u => u.TagId).
      Select(u => new
      {
        u.Key,
        InteractiveScore = u.Sum(v => v.Popularity)
      }).
      OrderBy(u => u.InteractiveScore).
      ToList();
      var tagRanks = tagGroups.Select((u, index) => new { TagId = u.Key, Rank = (decimal)index / tagGroups.Count }).ToDictionary(u => u.TagId, u => u.Rank);
      return tagRanks;
    }

    public override void Run()
    {
      Console.WriteLine($"Start calculating tag popularity at {DateTimeOffset.Now}");
      var coefficients = GetTagCorrelationCoefficients().ToDictionary(u => u.Tag);
      var maxCo = coefficients.Max(u => u.Value.Popularity);
      var tagPostRanks = GetTagRankDictionary();
      var updatedTags = new List<Tag>();
      var db = GetDbContext();
      foreach (var item in coefficients)
      {
        var tag = new Tag
        {
          Slug = item.Key,
          Popularity = item.Value.Popularity / maxCo * _tagPopularityWeight
          + tagPostRanks[item.Key] * _postNumberRankWeight
          + (decimal)(new Random().NextDouble() * _rngWeight)
        };
        db.Attach(tag).Property(u => u.Popularity).IsModified = true;
      }
      db.SaveChanges();
    }
  }
}