using System.Globalization;
using System;
using System.IO;
using DmcSocial.Models;
using CsvHelper;
using System.Linq;
using System.Collections.Generic;

namespace DmcSocial.Repositories
{
    public class DataSeeder
    {
        AppDbContext dbContext;
        public DataSeeder(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        Tag[] tags = new[]{
            new Tag {Value="system", IsSystemTag=true, NormalizeValue="system"},
            new Tag {Value="slide", IsSystemTag=true, NormalizeValue="slider"}
        };
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
    }
}