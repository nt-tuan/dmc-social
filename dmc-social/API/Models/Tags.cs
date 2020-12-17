using System;

namespace DmcSocial.API.Models
{
  public class Tag
  {
    public string Value { get; set; }
    public string Label { get; set; }
    public int PostCount { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public Tag(DmcSocial.Models.Tag tag)
    {
      Value = tag.Slug;
      Label = tag.Value;
      PostCount = tag.PostCount;
      LastModifiedAt = tag.LastModifiedTime;
    }
  }
}