using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace DmcSocial.Models
{
  public class GetListParams<T>
  {
    public enum OrderDirections { ASC = 0, DESC = 1 }
    [JsonPropertyName("offset")]
    public int Offset { get; set; } = 0;
    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 30;
    [JsonPropertyName("orderBy")]
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    [JsonPropertyName("orderDirection")]
    public OrderDirections OrderDirection { get; set; } = OrderDirections.ASC;
    [JsonPropertyName("at")]
    public DateTime At { get; set; } = DateTime.Now;
    public GetListParams() { }
    public GetListParams(int? offset, int? limit)
    {
      Offset = offset ?? 0;
      Limit = limit ?? 30;
      if (limit > 100)
        throw new Exception("limit-too-much");
    }

    public void SetGetAllItems()
    {
      Offset = 0;
      Limit = int.MaxValue;
    }
  }

  public class PostListParams : GetListParams<Post>
  {
    public PostListParams() { }
    public PostListParams(int? offset, int? limit, string orderBy, int? orderDir) : base(offset, limit)
    {
      this.OrderBy = post => post.Id;
      this.OrderDirection = OrderDirections.DESC;
      if (orderDir != null)
      {
        this.OrderDirection = (OrderDirections)orderDir;
      }
      if (orderBy == nameof(Post.Title))
      {
        this.OrderBy = post => post.Title;
      }
      else if (orderBy == nameof(Post.CreatedBy))
      {
        this.OrderBy = post => post.CreatedBy;
      }
      else if (orderBy == nameof(Post.ViewCount))
      {
        this.OrderBy = post => post.ViewCount;
      }
      else if (orderBy == nameof(Post.CommentCount))
      {
        this.OrderBy = post => post.CommentCount;
      }
    }
  }

  public class CommentListParams : GetListParams<PostComment>
  {
    public CommentListParams() { }
    public CommentListParams(int? offset, int? limit) : base(offset, limit)
    {
      OrderBy = comment => comment.Id;
      OrderDirection = OrderDirections.DESC;
    }
  }
}
