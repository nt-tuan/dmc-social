using System.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;

namespace DmcSocial.API.Models
{
  public class CommentResponse
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public int? PostId { get; set; }
    public int? CommentId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public CommentResponse(PostComment entity)
    {
      Id = entity.Id;
      Content = entity.Content;
      PostId = entity.PostId;
      CommentId = entity.ParentPostCommentId;
      CreatedBy = entity.CreatedBy;
      CreatedAt = entity.DateCreated;
      LastModifiedAt = entity.LastModifiedTime;
    }
  }
}