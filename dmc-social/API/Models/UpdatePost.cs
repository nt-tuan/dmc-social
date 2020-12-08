using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DmcSocial
{
  public class UpdatePostContent
  {
    [JsonPropertyName("content")]
    public string Content { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("subtitle")]
    public string Subtitle { get; set; }
    [JsonPropertyName("coverImageURL")]
    public string CoverImageURL { get; set; }
  }

  public class UpdatePostConfig
  {
    [JsonPropertyName("canComment")]
    public bool CanComment { get; set; }
    [JsonPropertyName("postRestrictionType")]
    public int PostRestrictionType { get; set; }
    [JsonPropertyName("accessUsers")]
    public string[] AccessUsers { get; set; }
  }
}