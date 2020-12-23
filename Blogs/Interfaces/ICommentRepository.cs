using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Entities;

namespace ThanhTuan.Blogs.Interfaces
{
  public interface ICommentRepository
  {
    Task<PostComment> CreatePostComment(int postId, int? commentId, string content, string by);
    Task<List<PostComment>> GetPostComments(int postId, GetListParams<PostComment> param);
    Task<List<PostComment>> GetSubPostComments(int commentId, GetListParams<PostComment> param);
    Task<int> GetPostCommentsCount(int postId);
    Task<int> GetSubPostCommentsCount(int commentId);
    Task DeleteComment(PostComment comment, string actor);
    Task<PostComment> GetPostCommentById(int id);
    Task<PostComment> UpdatePostComment(int id, string content, string actor);
  }
}