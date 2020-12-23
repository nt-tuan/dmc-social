using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class TagCorrellationCoefficientJob
  {
    class PairTag : IComparable<PairTag>
    {
      public string Tag1 { get; set; }
      public string Tag2 { get; set; }
      private string Hash()
      {
        return string.Compare(Tag1, Tag2) > 0 ? Tag1 + "|" + Tag2 : Tag2 + "|" + Tag1;
      }

      public int CompareTo([AllowNull] PairTag other)
      {
        if (other == null) return 1;
        return string.Compare(Hash(), other.Hash());
      }
      public class EqualityComparer : IEqualityComparer<PairTag>
      {
        public bool Equals(PairTag x, PairTag y)
        {
          return x.Hash() == y.Hash();
        }

        public int GetHashCode([DisallowNull] PairTag x)
        {
          return x.Hash().GetHashCode();
        }
      }
    }
    class TagCoefficient : PairTag
    {
      public double Coefficient { get; set; }
    }
    readonly DbContextOptionsBuilder<AppDbContext> _dbOptions;
    public TagCorrellationCoefficientJob()
    {
      _dbOptions = new DbContextOptionsBuilder<AppDbContext>();
      _dbOptions.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"));
    }
    List<Tag> GetTags()
    {
      var db = new AppDbContext(_dbOptions.Options);
      return db.Tags.AsNoTracking().ToList();
    }
    List<PostTag> GetPostTags()
    {
      var db = new AppDbContext(_dbOptions.Options);
      return db.PostTags.AsNoTracking().ToList();
    }
    Dictionary<PairTag, int> GetCoefficientMat(List<Tag> tags, List<PostTag> postTags)
    {
      Dictionary<PairTag, int> coefficientMat = new Dictionary<PairTag, int>(new PairTag.EqualityComparer());
      var index = 0;
      while (index < postTags.Count)
      {
        var post = postTags[index];
        var relatedTags = new List<string>();
        while (index < postTags.Count && postTags[index].PostId == post.PostId)
        {
          relatedTags.Add(postTags[index].TagId);
          index++;
        }
        for (var i = 0; i < relatedTags.Count; i++)
        {
          for (var j = i + 1; j < relatedTags.Count; j++)
          {
            var pair = new PairTag
            {
              Tag1 = relatedTags[i],
              Tag2 = relatedTags[j]
            };
            if (coefficientMat.ContainsKey(pair))
            {
              coefficientMat[pair]++;
            }
            else
            {
              coefficientMat[pair] = 1;
            }
          }
        }
      }
      return coefficientMat;
    }
    private void Save(List<TagCoefficient> correllationCos)
    {
      var db = new AppDbContext(_dbOptions.Options);
      db.RemoveRange(db.TagCorrelationCoefficients.AsQueryable());
      db.AddRange(correllationCos.Select(u => new TagCorrelationCoefficient
      {
        TagX = u.Tag1,
        TagY = u.Tag2,
        Coefficient = (decimal)u.Coefficient
      }));
      db.SaveChanges();
    }
    public void Run()
    {
      Console.WriteLine($"Start calculating tags correlation at {DateTimeOffset.Now}");
      var tags = GetTags();
      var tagDictionary = tags.ToDictionary(u => u.Slug);
      var postTags = GetPostTags().OrderBy(u => u.PostId).ToList();
      var coefficientMat = GetCoefficientMat(tags, postTags);
      var correlationCoefficient = new List<TagCoefficient>();
      foreach (var pair in coefficientMat)
      {
        var tagCoefficient = new TagCoefficient
        {
          Tag1 = pair.Key.Tag1,
          Tag2 = pair.Key.Tag2,
          Coefficient = pair.Value / (double)tagDictionary[pair.Key.Tag1].PostCount
        };
        var rTagCoefficient = new TagCoefficient
        {
          Tag1 = pair.Key.Tag2,
          Tag2 = pair.Key.Tag1,
          Coefficient = pair.Value / (double)tagDictionary[pair.Key.Tag2].PostCount
        };
        correlationCoefficient.Add(tagCoefficient);
        correlationCoefficient.Add(rTagCoefficient);
      }
      Save(correlationCoefficient);
    }
  }
}