using System.ComponentModel.DataAnnotations;

namespace ThanhTuan.Blogs.API.Models
{
  public class CreateComment
  {
    public int PostId { get; set; }
    public int? CommentId { get; set; }
    [MinLength(8)]
    [MaxLength(1024)]
    public string Content { get; set; }
  }
  public class UpdateComment
  {
    [MinLength(8)]
    [MaxLength(1024)]
    public string Content { get; set; }
  }
}