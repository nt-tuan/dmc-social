using System.Reflection.Metadata.Ecma335;
using System.Globalization;
using System;
using System.IO;
using DmcSocial.Models;
using CsvHelper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DmcSocial.Repositories
{
  public class DataSeeder
  {
    private readonly AppDbContext _dbContext;
    public DataSeeder(AppDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public void CorrectPosts()
    {
      var posts = _dbContext.Posts
      .Include(u => u.Comments)
      .ToList();
      foreach (var post in posts)
      {
        post.CommentCount = post.Comments.Where(u => u.DateRemoved == null).Count();
        _dbContext.Posts.Update(post);
      }
      _dbContext.SaveChanges();
    }

    public void CorrectComments()
    {
      var comments = _dbContext.PostComments.Include(u => u.ChildrenPostComments).ToList();
      foreach (var comment in comments)
      {
        comment.CommentCount = comment.ChildrenPostComments.Where(u => u.DateRemoved == null).Count();
        _dbContext.PostComments.Update(comment);
      }
      _dbContext.SaveChanges();
    }

    public void CorrectTags()
    {
      var tags = _dbContext.Tags.Where(u => u.DateRemoved == null).ToList();
      foreach (var tag in tags)
      {
        var postCount = _dbContext.PostTags.Where(postTag => postTag.TagId == tag.Value && postTag.Post.DateRemoved == null).Count();
        tag.PostCount = postCount;
      }
      _dbContext.UpdateRange(tags);
      _dbContext.SaveChanges();
    }

    public List<Tag> LoadTags()
    {
      var filepath = Environment.GetEnvironmentVariable("INIT_TAGS_FILE");
      if (string.IsNullOrEmpty(filepath))
      {
        return new List<Tag>();
      }
      try
      {
        using var reader = new StreamReader(filepath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Configuration.Delimiter = ";";
        csv.Configuration.HeaderValidated = null;
        csv.Configuration.MissingFieldFound = null;
        var records = csv.GetRecords<Tag>().ToList();
        return records;
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void Seed()
    {
      var tags = LoadTags();
      foreach (var tag in tags)
      {
        var e = _dbContext.Tags.Find(tag.Value);
        if (e == null)
        {
          tag.DateCreated = DateTime.Now;
          tag.CreatedBy = "system";
          _dbContext.Tags.Add(tag);
          _dbContext.SaveChanges();
        }
      }

      CorrectPosts();
      CorrectComments();
      CorrectTags();
    }
  }
}