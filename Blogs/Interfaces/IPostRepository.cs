using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Entities;

namespace ThanhTuan.Blogs.Interfaces
{
  public interface IPostRepository
  {
    Task<Post> CreatePost(Post post, string actor);
    Task<List<Post>> GetPosts(PostListParameter parameter);
    Task<int> CountPosts(PostCountParameter parameter);
    Task<List<Post>> SearchPosts(List<string> tagIds, List<string> keywords, int offset, int limit);
    Task<int> CountSearchedPosts(List<string> tagIds, List<string> keywords);
    Task<Post> GetPostById(int id, bool load = true);
    Task<PostMetric> GetPostMetricById(int id);
    Task<Post> UpdatePostContent(int postId, string title, string subtitle, string coverImageURL, string content, string actor);
    Task<Post> UpdatePostConfig(int postId, int PostRestrictionType, string[] PostAccessUsers, bool canComment);
    Task DeletePost(int postId, string actor);
    Task AddTag(Post entity, string tag, string actor);
    Task RemoveTag(Post entity, string tag, string actor);
    Task<int> IncreaseView(int postId);
    Task<List<GroupByAuthor>> GetPostsGroupByAuthor(PagingParameter<GroupByAuthor> paging);
  }
}
