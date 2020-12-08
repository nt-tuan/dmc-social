using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DmcSocial.Models
{
  public class GetListParams<T>
  {
    public enum OrderDir { ASC = 0, DESC = 1 }
    public int offset { get; set; } = 0;
    public int limit { get; set; } = 30;
    public Expression<Func<T, object>> orderBy { get; set; } = null;
    public OrderDir orderDir { get; set; } = OrderDir.ASC;
    public DateTime at { get; set; } = DateTime.Now;
    public GetListParams() { }
    public GetListParams(int? offset, int? limit)
    {
      this.offset = offset ?? 0;
      this.limit = limit ?? 30;
      if (limit > 100)
        throw new Exception("limit-too-much");
    }

    public int GetSkipNumber()
    {
      return offset;
    }

    public void SetGetAllItems()
    {
      offset = 0;
      limit = int.MaxValue;
    }
  }

  public class PostListParams : GetListParams<Post>
  {
    public PostListParams(int? offset, int? limit, string orderBy, int? orderDir) : base(offset, limit)
    {
      this.orderBy = post => post.Id;
      this.orderDir = OrderDir.DESC;
      if (orderDir != null)
      {
        this.orderDir = (OrderDir)orderDir;
      }
      if (orderBy == nameof(Post.Subject))
      {
        this.orderBy = post => post.Subject;
      }
      else if (orderBy == nameof(Post.CreatedBy))
      {
        this.orderBy = post => post.CreatedBy;
      }
      else if (orderBy == nameof(Post.ViewCount))
      {
        this.orderBy = post => post.ViewCount;
      }
      else if (orderBy == nameof(Post.CommentCount))
      {
        this.orderBy = post => post.CommentCount;
      }
    }
  }

  public class CommentListParams : GetListParams<PostComment>
  {
    public CommentListParams(int? offset, int? limit) : base(offset, limit)
    {
      orderBy = comment => comment.Id;
      orderDir = OrderDir.DESC;
    }
  }
}
