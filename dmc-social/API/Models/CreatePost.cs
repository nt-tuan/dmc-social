using System.Linq;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DmcSocial
{
  public class CreatePost
  {
    [MinLength(32)]
    [MaxLength(4048)]
    public string Content { get; set; }
    [MinLength(8)]
    [MaxLength(255)]
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public bool CanComment { get; set; }
    public int PostRestrictionType { get; set; }
    public string[] AccessUsers { get; set; }
    public string[] Tags { get; set; }
    public string CoverImageURL { get; set; }
    public virtual Models.Post ToEntity()
    {
      var post = new Models.Post
      {
        Content = Content,
        Title = Title,
        CanComment = CanComment,
        DateCreated = DateTimeOffset.Now,
        Subtitle = Subtitle,
        CoverImageURL = CoverImageURL
      };
      if (Tags != null)
      {
        post.PostTags = Tags.Select(tag => new Models.PostTag
        {
          TagId = tag
        }).ToList();
      }

      post.PostRestrictionType = PostRestrictionType;
      if (AccessUsers != null)
      {
        post.PostAccessUsers = AccessUsers;
      }
      return post;
    }
  }
}