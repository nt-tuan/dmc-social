using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DmcSocial.Repositories;
using System.Collections.Generic;
using DmcSocial.Models;
using DmcSocial.API.Models;
using System.Linq;

namespace DmcSocial.API
{
  [Route("comment")]
  [ApiController]
  public class CommentController : ControllerBase
  {
    ICommentRepository _repos;
    Authenticate _auth;
    public CommentController(Authenticate auth, ICommentRepository repos)
    {
      _repos = repos;
      _auth = auth;
    }

    [HttpGet]
    [Route("post/{postId}")]
    public async Task<ActionResult<List<CommentResponse>>> GetPostComments(int postId, int pageIndex, int pageRows)
    {
      var comments = await _repos.GetPostComments(postId,
      new GetListParams<PostComment> { page = pageIndex, pageRows = pageRows });
      return comments.Select(u => new CommentResponse(u)).ToList();
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
    public async Task<ActionResult<List<CommentResponse>>> GetSubPostComments(int commentId, int pageIndex, int pageRows)
    {
      var comments = await _repos.GetSubPostComments(commentId,
      new GetListParams<PostComment> { page = pageIndex, pageRows = pageRows });
      return comments.Select(u => new CommentResponse(u)).ToList();
    }

    [HttpGet]
    [Route("{commentId}/comments/count")]
    public async Task<ActionResult<int>> GetSubPostCommentsCount(int commentId)
    {
      var count = await _repos.GetSubPostCommentsCount(commentId);
      return count;
    }

    [HttpPost]
    public async Task<ActionResult<CommentResponse>> CreateComment(CreateComment req)
    {
      var comment = new PostComment
      {
        Content = req.content,
        PostId = req.postId,
        ParentPostCommentId = req.commentId,
        DateCreated = DateTime.Now,
        CreatedBy = _auth.GetUser()
      };
      var entity = await _repos.CreatePostComment(comment);
      return new CommentResponse(entity);
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