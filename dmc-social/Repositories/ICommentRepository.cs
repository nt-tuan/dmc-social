using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DmcSocial.Models;

namespace DmcSocial.Repositories
{
    public interface ICommentRepository
    {
        Task<PostComment> CreatePostComment(PostComment comment);
        Task<List<PostComment>> GetPostComments(int postId, GetListParams<PostComment> param);
        Task<List<PostComment>> GetSubPostComments(int commentId, GetListParams<PostComment> param);
        Task<int> GetPostCommentsCount(int postId);
        Task<int> GetSubPostCommentsCount(int commentId);
        Task DeleteComment(PostComment comment);
        Task<PostComment> GetPostCommentById(int id);
    }
}