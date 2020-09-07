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
        AppDbContext dbContext;
        ILogger logger;
        public DataSeeder(AppDbContext dbContext, ILoggerFactory factory)
        {
            this.dbContext = dbContext;
            logger = factory.CreateLogger(typeof(DataSeeder));
        }
        Tag[] tags = new[]{
            new Tag {Value="system", IsSystemTag=true, NormalizeValue="system"},
            new Tag {Value="slide", IsSystemTag=true, NormalizeValue="slider"}
        };

        public void CorrectPosts()
        {
            var posts = dbContext.Posts
            .Include(u => u.Comments)
            .ToList();
            foreach (var post in posts)
            {
                post.CommentCount = post.Comments.Where(u => u.DateRemoved == null).Count();
                dbContext.Posts.Update(post);
            }
            dbContext.SaveChanges();
        }

        public void CorrectComments()
        {
            var comments = dbContext.PostComments.Include(u => u.ChildrenPostComments).ToList();
            foreach (var comment in comments)
            {
                comment.CommentCount = comment.ChildrenPostComments.Where(u => u.DateRemoved == null).Count();
                dbContext.PostComments.Update(comment);
            }
            dbContext.SaveChanges();
        }

        public void CorrectTags()
        {
            var tags = dbContext.Tags.Where(u => u.DateRemoved == null).ToList();
            foreach (var tag in tags)
            {
                var postCount = dbContext.PostTags.Where(postTag => postTag.TagId == tag.Value && postTag.Post.DateRemoved == null).Count();
                tag.PostCount = postCount;
            }
            dbContext.UpdateRange(tags);
            dbContext.SaveChanges();
        }

        public List<Tag> loadTags()
        {
            var filepath = Environment.GetEnvironmentVariable("INIT_TAGS_FILE");
            if (string.IsNullOrEmpty(filepath))
            {
                return new List<Tag>();
            }
            try
            {
                using (var reader = new StreamReader(filepath))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Configuration.Delimiter = ";";
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.MissingFieldFound = null;
                        var records = csv.GetRecords<Tag>().ToList();
                        return records;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Seed()
        {
            var tags = loadTags();
            foreach (var tag in tags)
            {
                var e = dbContext.Tags.Find(tag.Value);
                if (e == null)
                {
                    tag.DateCreated = DateTime.Now;
                    tag.CreatedBy = "system";
                    dbContext.Tags.Add(tag);
                    dbContext.SaveChanges();
                }
            }

            CorrectPosts();
            CorrectComments();
            CorrectTags();
        }
    }
}