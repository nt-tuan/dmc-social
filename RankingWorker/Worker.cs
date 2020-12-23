using System.Threading.Tasks.Dataflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ThanhTuan.Blogs.RankingWorker
{
  public class Worker : BackgroundService
  {
    private readonly ILogger<Worker> _logger;
    private readonly TimeSpan _interval;
    public Worker(ILogger<Worker> logger)
    {
      _logger = logger;
      _interval = TimeSpan.Parse(Environment.GetEnvironmentVariable("RANKING_INTERVAL"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        DoWork();
        var nextRunAt = DateTimeOffset.Now + _interval;
        _logger.LogInformation("Next run at: {time}, delays {delay}", nextRunAt, _interval.TotalMilliseconds);
        await Task.Delay((int)_interval.TotalMilliseconds, stoppingToken);
      }
    }

    private void DoWork()
    {
      var job = new Jobs.RankingJob();
      job.Run();
    }
  }
}
