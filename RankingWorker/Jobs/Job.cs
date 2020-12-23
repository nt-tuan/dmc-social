using System;
using ThanhTuan.Blogs.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.RankingWorker.Jobs
{
  public abstract class Job
  {
    public abstract void Run();
    public static string GetDatabaseURL()
    {
      return Environment.GetEnvironmentVariable("DATABASE_URL");
    }
    public static AppDbContext GetDbContext()
    {
      var builder = new DbContextOptionsBuilder<AppDbContext>();
      builder.UseNpgsql(GetDatabaseURL(), builder =>
      {
        builder.CommandTimeout(120);
      });

      return new AppDbContext(builder.Options);
    }
  }
}