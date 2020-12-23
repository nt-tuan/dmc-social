using System;

namespace ThanhTuan.Blogs.Entities
{
  public class IndexingSchedule
  {
    public int Id { get; set; }
    public const string IndexingSuccess = "success";
    public const string IndexingPending = "pending";
    public const string IndexingFailed = "failed";
    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public DateTimeOffset RunAt { get; set; }
    public string State { get; set; }
    public string Message { get; set; }
  }
}