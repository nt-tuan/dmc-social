using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DmcSocial
{
  public class UpdatePostContent
  {
    [MinLength(32)]
    [MaxLength(4048)]
    public string Content { get; set; }
    [MinLength(8)]
    [MaxLength(255)]
    public string Title { get; set; }

    public string Subtitle { get; set; }

    public string CoverImageURL { get; set; }
  }

  public class UpdatePostConfig
  {

    public bool CanComment { get; set; }

    public int PostRestrictionType { get; set; }

    public string[] AccessUsers { get; set; }
  }
}