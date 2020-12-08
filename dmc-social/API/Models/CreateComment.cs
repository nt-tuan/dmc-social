namespace DmcSocial.API.Models
{
  public class CreateComment
  {
    public int PostId { get; set; }
    public int? CommentId { get; set; }
    public string Content { get; set; }
  }
}