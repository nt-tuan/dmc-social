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
    public PostFilter(List<string> filterBy, List<string> filterValue)
    {
      FilterQueries = new List<Expression<Func<Post, bool>>>();
      if (filterBy == null || filterValue == null) return;
      var numFilter = new int[] { filterBy.Count, filterValue.Count }.Min();
      for (var index = 0; index < numFilter; index++)
      {
        if (filterBy[index] == nameof(Post.CreatedBy))
        {
          FilterQueries.Add(post => post.CreatedBy.ToLower().Contains(filterValue[index].ToLower()));
          continue;
        }
      }
    }
  }
  public class CountParameter<T> : Filterable<T>
  {
    public PostFilter Filter { get; set; }
  }
  public class ListParameter<T>
  {
    public Filterable<T> Filter { get; set; }
    public enum OrderDirections { ASC = 0, DESC = 1 }
    public int Offset { get; set; } = 0;
    public int Limit { get; set; } = 30;
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    public OrderDirections OrderDirection { get; set; } = OrderDirections.ASC;
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
      OrderDirection = OrderDirections.DESC;
      if (orderDir != null)
      {
        OrderDirection = (OrderDirections)orderDir;
      }
      OrderBy = ParseOrderBy(orderBy);
    }
  }
  public class CommentListParameter : ListParameter<PostComment>
  {
  }
}
