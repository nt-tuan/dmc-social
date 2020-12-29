using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.Repositories;
using Npgsql;
using PostgreSQLCopyHelper;

namespace ThanhTuan.Blogs.RankingWorker
{
  public static class RankingUtilities
  {
    private static readonly int _titleWeight = 100;
    public static void BulkInsertWordFrequencies(List<WordFrequency> entities, string dbURL)
    {
      var copyHelper = new PostgreSQLCopyHelper<WordFrequency>("\"WordFrequencies\"")
      .MapInteger("\"PostId\"", x => x.PostId)
      .MapBigInt("\"Frequency\"", x => x.Frequency)
      .MapText("\"Word\"", x => x.Word);
      using (var connection = new NpgsqlConnection(dbURL))
      {
        connection.Open();
        copyHelper.SaveAll(connection, entities);
        connection.Close();
      }
    }
    public static Dictionary<string, long> ExtractWordFrequency(string text)
    {
      var words = Helper.NormalizeString(text).Split("_");
      var dict = new Dictionary<string, long>();
      foreach (var word in words)
      {
        if (word.Length < 2) continue;
        if (dict.ContainsKey(word))
        {
          dict[word]++;
        }
        else
        {
          dict[word] = 1;
        }
      }
      return dict;
    }
    public static List<WordFrequency> GetWordFrequencies(Post post)
    {
      var dict = ExtractWordFrequency(post.Content);
      var titleDict = ExtractWordFrequency(post.Title);
      foreach (var pair in titleDict)
      {
        var value = pair.Value * _titleWeight;
        if (dict.ContainsKey(pair.Key))
        {
          dict[pair.Key] += value;
        }
        else
        {
          dict[pair.Key] = value;
        }
      }
      return dict.Select(pair =>
      {
        return new WordFrequency
        {
          Word = pair.Key,
          Frequency = pair.Value,
          PostId = post.Id
        };
      }).ToList();
    }
  }
}