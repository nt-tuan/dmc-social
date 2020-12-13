using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DmcSocial.Models
{
  public class Tag : BaseEntity
  {
    public string Value { get; set; }
    public string Reference { get; set; }
    public int PostCount { get; set; }
    public string NormalizeValue { get; set; }
    public Tag() : base() { }
    public Tag(string tag, string actor) : base(actor)
    {
      Value = tag;
      NormalizeValue = "";
      if (string.IsNullOrEmpty(tag))
        return;
      NormalizeValue = Repositories.Helper.NormalizeTag(tag);
    }
  }
}
