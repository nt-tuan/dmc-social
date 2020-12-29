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
using System.Threading;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public class MediumArticle
  {
    public string name { get; set; }
    public string userName { get; set; }
    public string url { get; set; }
    public string title { get; set; }
    public string text { get; set; }
    public string subTitle { get; set; }
  }
  public class AddMediumPostJob : Job
  {
    readonly string _filePath;
    public AddMediumPostJob()
    {
      _filePath = Environment.GetEnvironmentVariable("FILE");
    }
    public override void Run()
    {
      AddTags("Medium");
      for (var i = 0; i < 3; i++)
      {
        Console.WriteLine();
      }
      using (FileStream fs = File.Open(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BufferedStream bs = new BufferedStream(fs))
        {
          using (StreamReader sr = new StreamReader(bs))
          {
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
              csv.Read();
              csv.ReadHeader();
              var count = 0;
              while (csv.Read())
              {
                // if (count > 10) break;
                var record = csv.GetRecord<MediumArticle>();
                Console.SetCursorPosition(0, Console.CursorTop - 3);
                Console.Write("- ");
                AddPost(record);
                count++;
                var progress = (double)sr.BaseStream.Position / sr.BaseStream.Length;
                Console.WriteLine(new string(' ', Console.BufferWidth));
                var progStr = $"Progress: ${progress * 100}";
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(progStr);
              }
            }
          }
        }
      }
      Console.WriteLine();
    }
    private Tag AddTags(string tagName)
    {
      var tag = new Tag(tagName, "admin");
      var db = GetDbContext();
      if (db.Tags.Any(dbTag => dbTag.Slug == tag.Slug))
        return tag;
      var tagRepo = new TagRepository(GetDbContext());
      var mediumTag = tagRepo.AddTag(tag, "admin").Result;
      return tag;
    }
    private void AddPostTags(MediumArticle record, Post post)
    {
      var db = GetDbContext();
      var repo = new PostRepository(db);
      if (!string.IsNullOrEmpty(record.name))
      {
        var tag = AddTags(record.name);
        try
        {
          repo.AddTag(post, tag.Slug, "admin").Wait();
        }
        catch (Exception e)
        {
          Console.WriteLine($"Add tag {tag.Slug} failed with message: {e.Message}");
        }
      }
      repo.AddTag(post, "medium", "admin").Wait();
    }
    private void AddPost(MediumArticle record)
    {
      var db = GetDbContext();
      var first = db.Posts.Where(u => u.DateRemoved == null && u.Title == record.title).FirstOrDefault();
      if (first != null)
      {
        AddPostTags(record, first);
        Console.WriteLine($"Post {record.title} existed");
        return;
      }
      if (string.IsNullOrEmpty(record.name))
      {
        Console.WriteLine($"Post {record.title} has no tag. Skiped!");
        return;
      }
      var repo = new PostRepository(db);
      var lines = record.text.Split(Environment.NewLine, 4);
      var subtitle = $"Reference: {record.url}{Environment.NewLine}{record.subTitle}";
      var text = $"Reference: {record.url}{Environment.NewLine}{record.text}";
      var post = repo.CreatePost(new Post
      {
        Title = record.title,
        Content = record.text,
        Subtitle = subtitle,
      }, Helper.NormalizeString(record.userName)).Result;
      AddPostTags(record, post);
      Console.WriteLine($"Create post#{post.Id}, {post.Title} successfull");
    }
  }
}





