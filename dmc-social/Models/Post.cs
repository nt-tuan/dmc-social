using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Models
{
  public class Post : BaseEntity
  {
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool CanComment { get; set; }
    #region config
    public int PostRestrictionType { get; set; }
    public string[] PostAccessUsers { get; set; }
    #endregion

    //META DATA
    public DateTime LastModifiedDate { get; set; }

    #region metric
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
    #endregion
    //REFERENCES    
    public ICollection<PostComment> Comments { get; set; }

    public ICollection<PostTag> PostTags { get; set; }
  }

  public class PostMetric
  {
    public int ViewCount { get; set; }
    public int CommentCount { get; set; }
    public PostMetric(Post post)
    {
      ViewCount = post.ViewCount;
      CommentCount = post.CommentCount;
    }

  }

  // public class PostConfig
  // {
  //     public enum PostRestriction { NONE, ALLOW_USERS }
  //     public int PostRestrictionType { get; set; }
  //     public string[] PostAccessUsers { get; set; }
  //     public PostConfig(Post post)
  //     {
  //         PostRestrictionType = post.PostRestrictionType;
  //         PostAccessUsers = post.PostAccessUsers;
  //     }
  //     public PostRestriction GetPostRestriction()
  //     {
  //         return (PostRestriction)PostRestrictionType;
  //     }

  //     public void SetPostRestriction(PostRestriction restriction)
  //     {
  //         PostRestrictionType = (int)restriction;
  //     }
  // }
}
