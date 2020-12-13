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

namespace scripts
{
  public class MediumArticle
  {
    public string author { get; set; }
    public string claps { get; set; }
    public string reading_time { get; set; }
    public string link { get; set; }
    public string title { get; set; }
    public string text { get; set; }
  }
  public class AddMediumPost
  {
    public void Run(string[] args)
    {
      // var dbURL = "Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3";
      var dbURL = args[0];
      var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
      optionsBuilder.UseNpgsql(dbURL);
      var path = args[1];
      AddTags(optionsBuilder);
      using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        using (BufferedStream bs = new BufferedStream(fs))
        {
          using (StreamReader sr = new StreamReader(bs))
          {
            using (var csv = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
              csv.Read();
              csv.ReadHeader();
              while (csv.Read())
              {
                var record = csv.GetRecord<MediumArticle>();
                AddPost(record, optionsBuilder);
              }
            }
          }
        }
      }
    }
    private void AddTags(DbContextOptionsBuilder<AppDbContext> optionsBuilder)
    {
      var db = new AppDbContext(optionsBuilder.Options);
      if (db.Tags.Any(tag => tag.NormalizeValue == "medium"))
        return;
      var tagRepo = new TagRepository(new AppDbContext(optionsBuilder.Options));
      var mediumTag = tagRepo.AddTag(new Tag("medium", "admin"), "admin").Result;
    }
    private void AddPost(MediumArticle record, DbContextOptionsBuilder<AppDbContext> optionsBuilder)
    {
      var db = new AppDbContext(optionsBuilder.Options);
      var exists = db.Posts.Where(u => u.DateRemoved == null && u.Title == record.title).Any();
      if (exists)
      {
        Console.WriteLine($"Post {record.title} existed");
        return;
      }
      var repo = new PostRepository(db);
      var lines = record.text.Split("\n", 4);
      var subtitle = $@"Reference: {record.link}
        {string.Join("\n", lines.Take(lines.Length - 1))}
        ";
      var text = $"Reference: ${record.link}\n${record.text}";
      var post = repo.CreatePost(new DmcSocial.Models.Post
      {
        Title = record.title,
        Content = record.text,
        Subtitle = subtitle,
      }, Helper.NormalizeString(record.author)).Result;
      repo.AddTag(post, "medium", "admin").Wait();
      Console.WriteLine($"Create post#{post.Id}, {post.Title} successfull");
    }
  }
}





