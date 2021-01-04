using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace ThanhTuan.Blogs.Entities
{
  public class Filterable<T>
  {
    public List<Expression<Func<T, bool>>> FilterQueries { get; set; } = null;
  }
  public class PostFilter : Filterable<Post>
  {
    public PostFilter()
    {
      FilterQueries = new List<Expression<Func<Post, bool>>>();
    }
  }
  public class CountParameter<T> : Filterable<T>
  {
    public PostFilter Filter { get; set; }
  }
  public class PagingParameter<T>
  {
    public enum OrderDirections { ASC = 0, DESC = 1 }
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 30;
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    public OrderDirections OrderDirection { get; set; } = OrderDirections.ASC;
  }
  public class ListParameter<T>
  {
    public Filterable<T> Filter { get; set; }
    public enum OrderDirections { ASC = 0, DESC = 1 }
    public PagingParameter<T> Paging { get; set; }
  }

  public class PostCountParameter
  {
    public Filterable<Post> Filter { get; set; }
    public List<string> Tags { get; set; }
  }
  public class PostListParameter : ListParameter<Post>
  {
    public List<string> Tags { get; set; }
    public PostListParameter() { }
    public Expression<Func<Post, object>> ParseOrderBy(string orderBy)
    {
      return orderBy switch
      {
        nameof(Post.Title) => post => post.Title,
        nameof(Post.CreatedBy) => post => post.CreatedBy,
        nameof(Post.DateCreated) => post => post.DateCreated,
        nameof(Post.ViewCount) => post => post.ViewCount,
        nameof(Post.CommentCount) => post => post.CommentCount,
        _ => post => post.Popularity
      };
    }
    public void SetOrder(string orderBy, int? orderDir)
    {
      Paging.OrderDirection = PagingParameter<Post>.OrderDirections.DESC;
      if (orderDir != null)
      {
        Paging.OrderDirection = (PagingParameter<Post>.OrderDirections)orderDir;
      }
      Paging.OrderBy = ParseOrderBy(orderBy);
    }
  }
  public class CommentListParameter : ListParameter<PostComment>
  {
  }
}
