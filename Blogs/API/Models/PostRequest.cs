using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using ThanhTuan.Blogs.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ThanhTuan.Blogs.API.Models
{
  public class UpdatePostContent
  {
    [MinLength(32)]
    [MaxLength(20000)]
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
  public class CreatePost
  {
    [MinLength(32)]
    [MaxLength(20000)]
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
    public virtual Post ToEntity()
    {
      var post = new Post
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
        post.PostTags = Tags.Select(tag => new PostTag
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

  public class PostFilterQuery
  {
    [FromQuery]
    public string[] Tags { get; set; }
    [FromQuery]
    public string CreatedBy { get; set; }
    public PostCountParameter ToParameter()
    {
      var parameter = new PostCountParameter
      {
        Filter = new PostFilter(),
        Tags = Tags?.ToList(),
      };
      if (!string.IsNullOrEmpty(CreatedBy))
      {
        parameter.Filter.FilterQueries.Add(post => post.CreatedBy == CreatedBy);
      }
      return parameter;
    }
  }
  public class GetPostQuery : PagingQuery
  {
    [FromQuery]
    public string[] Tags { get; set; }
    [FromQuery]
    public string TagFilterType { get; set; }
    [FromQuery]
    public string CreatedBy { get; set; }
    public PostListParameter ToParameter()
    {
      var parameter = new PostListParameter
      {
        Paging = new PagingParameter<Post>
        {
          Offset = Offset,
          Limit = Limit,
        },
        Tags = Tags?.ToList(),
        Filter = new PostFilter()
      };
      if (!string.IsNullOrEmpty(CreatedBy))
      {
        parameter.Filter.FilterQueries.Add(post => post.CreatedBy == CreatedBy);
      }
      parameter.SetOrder(By, Dir);
      return parameter;
    }
  }

  public class SearchPostQuery : GetPostQuery
  {
    [FromQuery]
    public string[] Keywords { get; set; }
  }
}