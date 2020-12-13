using System;

namespace DmcSocial.API.Models
{
  public class Tag
  {
    public string Value { get; set; }
    public int PostCount { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
  }
}