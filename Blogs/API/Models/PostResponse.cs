using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Entities;

namespace ThanhTuan.Blogs.API.Models
{
  public class PostListItemResponse
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public string CoverImageURL { get; set; }
    public string[] Tags { get; set; }
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
    public PostListItemResponse(Post entity)
    {
      Id = entity.Id;
      Title = entity.Title;
      LastModifiedAt = entity.LastModifiedTime;
      CoverImageURL = entity.CoverImageURL;
      Subtitle = entity.Subtitle;
      LastModifiedBy = entity.LastModifiedBy;
      CreatedBy = entity.CreatedBy;
      CreatedAt = entity.DateCreated;
      ViewCount = entity.ViewCount;
      CommentCount = entity.CommentCount;
      if (entity.PostTags != null)
      {
        Tags = entity.PostTags.Select(u => u.TagId).ToArray();
      }
    }
  }
  public class PostResponse : PostListItemResponse
  {
    public string Content { get; set; }
    public bool CanComment { get; set; }


    //REFERENCES
    // public PostConfig Config { get; set; }
    public int PostRestrictionType { get; set; }
    public string[] PostAccessUsers { get; set; }
    public PostResponse(Post entity) : base(entity)
    {
      Content = entity.Content;
      CanComment = entity.CanComment;
      PostRestrictionType = entity.PostRestrictionType;
      PostAccessUsers = entity.PostAccessUsers;
    }
  }
}