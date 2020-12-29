using System.Reflection;
using System;
using Xunit;
using ThanhTuan.Blogs.Entities;
using Console.Jobs;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Blogs.Repositories;
using System.Linq;
using ThanhTuan.Blogs.RankingWorker.Jobs;

namespace Test.Repositories
{
  public class TagFixMediumPost
  {
    [Fact]
    public void TestFixPost()
    {
      Environment.SetEnvironmentVariable("DATABASE_URL", "Host=localhost;Database=dmcsocial;Username=dmcsocial;Password=hala29an3");
      var job = new FixMediumPostJob();
      var post = Job.GetDbContext().Posts.First(u => u.Subtitle.StartsWith("Reference: http"));
      job.FixPostNewLine(post);
    }
  }
}