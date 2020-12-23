using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ThanhTuan.Blogs.Entities
{
  public class Tag : BaseEntity
  {
    public string Value { get; set; }
    public string Reference { get; set; }
    public int PostCount { get; set; }
    public decimal Popularity { get; set; }
    public string Slug { get; set; }
    public Tag() : base() { }
    public Tag(string tag, string actor) : base(actor)
    {
      Value = tag;
      Slug = "";
      if (string.IsNullOrEmpty(tag))
        return;
      Slug = Repositories.Helper.NormalizeTag(tag);
    }
  }
}
