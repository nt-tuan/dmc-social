using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DmcSocial.Models;

namespace DmcSocial.Interfaces
{
  public interface IPostRepository
  {
    Task<Post> CreatePost(Post post, string actor);
    Task<List<Post>> GetPosts(List<string> tags, GetListParams<Post> param);
    Task<int> CountPosts(List<string> tags);
    Task<List<Post>> SearchPosts(List<string> tagIds, GetListParams<Post> param);
    Task<Post> GetPostById(int id, bool load = true);
    Task<PostMetric> GetPostMetricById(int id);
    Task<Post> UpdatePostContent(int postId, string subject, string content, string actor);
    Task<Post> UpdatePostConfig(int postId, int PostRestrictionType, string[] PostAccessUsers);
    Task DeletePost(int postId, string actor);
    Task AddTag(Post entity, string tag);
    Task RemoveTag(Post entity, string tag);
    Task IncreaseView(int postId);
  }
}
