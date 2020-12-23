using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ThanhTuan.Blogs.Entities;
namespace ThanhTuan.Blogs.API.Models
{
  public class TagPayload
  {
    public string Value { get; set; }
    public string Label { get; set; }
    public int PostCount { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public TagPayload(Tag tag)
    {
      Value = tag.Slug;
      Label = tag.Value;
      PostCount = tag.PostCount;
      LastModifiedAt = tag.LastModifiedTime;
    }
  }
  public class GetBatchTags : IValidatableObject
  {
    [FromQuery]
    public List<string> Tags { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (Tags == null || Tags.Count > 100)
      {
        yield return new ValidationResult("too-many-tags");
      }
    }
  }
  public class GetTagsQuery : PagingQuery
  {
    [FromQuery]
    public string Search { get; set; }
  }
  public class GetRelatedTagQuery
  {
    [FromQuery]
    public string Tag { get; set; }
    [FromQuery]
    public int Offset { get; set; }
    [FromQuery]
    [Range(0, 100)]
    public int Limit { get; set; }
  }
}