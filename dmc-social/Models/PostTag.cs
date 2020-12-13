using System;

namespace DmcSocial.Models
{
  public class PostTag : BaseEntity
  {
    public int PostId { get; set; }
    public Post Post { get; set; }
    public string TagId { get; set; }
    public Tag Tag { get; set; }
    public PostTag() : base() { }
    public PostTag(string actor) : base(actor) { }
  }
}
