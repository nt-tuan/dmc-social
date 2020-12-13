using System.Collections.Generic;

namespace DmcSocial.Models
{
  public class PostComment : BaseEntity
  {
    public string Content { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
    public int? ParentPostCommentId { get; set; }
    public int CommentCount { get; set; }
    public PostComment ParentPostComment { get; set; }
    public ICollection<PostComment> ChildrenPostComments { get; set; }
    public PostComment() : base() { }
    public PostComment(string actor) : base(actor) { }
  }
}
