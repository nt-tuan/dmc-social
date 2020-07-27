using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DmcSocial.Repositories;
using System.Collections.Generic;
using DmcSocial.Models;
using DmcSocial.API.Models;

namespace DmcSocial.API
{
    [Route("comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        ICommentRepository _repos;
        public CommentController(ICommentRepository repos)
        {
            _repos = repos;
        }

        [HttpGet]
        [Route("post/{postId}")]
        public async Task<ActionResult<List<PostComment>>> GetPostComments(int postId, int pageIndex, int pageRows)
        {
            var comments = await _repos.GetPostComments(postId,
            new GetListParams<PostComment> { page = pageIndex, pageRows = pageRows });
            return comments;
        }

        [HttpGet]
        [Route("post/{postId}/count")]
        public async Task<ActionResult<int>> GetPostCommentsCount(int postId)
        {
            var count = await _repos.GetPostCommentsCount(postId);
            return count;
        }

        [HttpGet]
        [Route("{commentId}/comments")]
        public async Task<ActionResult<List<PostComment>>> GetSubPostComments(int commentId, int pageIndex, int pageRows)
        {
            var comments = await _repos.GetPostComments(commentId,
            new GetListParams<PostComment> { page = pageIndex, pageRows = pageRows });
            return comments;
        }

        [HttpGet]
        [Route("{commentId}/comments/count")]
        public async Task<ActionResult<int>> GetSubPostCommentsCount(int commentId)
        {
            var count = await _repos.GetSubPostCommentsCount(commentId);
            return count;
        }

        [HttpPut]
        public async Task<ActionResult<PostComment>> CreateComment(PostComment req)
        {
            var comment = new PostComment
            {
                Content = req.Content,
                PostId = req.PostId,
                ParentPostCommentId = req.ParentPostCommentId,
                DateCreated = DateTime.Now
            };
            return await _repos.CreatePostComment(comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _repos.GetPostCommentById(id);
            if (comment == null) return NotFound();
            await _repos.DeleteComment(comment);
            return Ok();
        }
    }
}