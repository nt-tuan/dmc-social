using System.ComponentModel.Design;
using ThanhTuan.Blogs.RankingWorker.Jobs;
namespace Console
{
  class Program
  {
    static void Main(string[] args)
    {
      var watch = System.Diagnostics.Stopwatch.StartNew();
      Run(args[0]);
      watch.Stop();
      System.Console.WriteLine($"Job done in {watch.ElapsedMilliseconds} ms");
    }
    static void Run(string name)
    {
      switch (name)
      {
        case "calc-words":
          new WordFrequencyJob().Run();
          return;
        case "add-posts":
          new AddMediumPostJob().Run();
          return;
        case "calc-tag-correlation":
          new TagCorrellationCoefficientJob().Run();
          return;
        case "calc-tag-popularity":
          new TagPopularityJob().Run();
          return;
        case "calc-post-popularity":
          new PostPopularityJob().Run();
          return;
        case "rank":
          new RankingJob().Run();
          return;
      }
    }

  }
}
