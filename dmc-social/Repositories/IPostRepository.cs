using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DmcSocial.Models;

namespace DmcSocial.Repositories
{
    public interface IPostRepository
    {
        Task<Post> CreatePost(Post post);
        Task<List<Post>> GetPosts(List<string> tags, GetListParams<Post> param);
        Task MarkPostViewed(Post post);
        Task<int> CountPosts(List<string> tags);
        Task<List<Post>> SearchPosts(List<string> tagIds, GetListParams<Post> param);
        Task<Post> GetPostById(int id, bool load = true);
        Task<Post> UpdatePostContent(int postId, string subject, string title, string actor);
        Task<Post> UpdatePostConfig(Post entity);
        Task DeletePost(Post entity);
        Task AddTag(Post entity, string tag);
        Task RemoveTag(Post entity, string tag);
    }
}
