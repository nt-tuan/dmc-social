using System.Net.Cache;
using System.Linq;
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DmcSocial.Interfaces;
using System.Collections.Generic;
using DmcSocial.Models;
using DmcSocial.API.Models;
using DmcSocial.Repositories;
using System.Linq.Expressions;

namespace DmcSocial.API
{
  [Route("post")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostRepository _repo;
    private readonly Authenticate _auth;
    public PostController(IPostRepository repo, Authenticate auth)
    {
      _repo = repo;
      _auth = auth;
    }

    /// <summary>
    /// Get posts
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<PostResponse>>> GetPosts(int? offset, int? limit, string by, int? dir, [FromQuery] string[] tags)
    {
      var paging = new PostListParams(offset, limit, by, dir);
      var posts = await _repo.GetPosts(tags.ToList(), paging);
      return Ok(posts.Select(u => new PostResponse(u)).ToList());
    }

    [HttpGet]
    [Route("count")]
    public async Task<ActionResult<int>> CountPosts([FromQuery] string[] tags)
    {
      var total = await _repo.CountPosts(tags.ToList());
      return total;
    }

    [HttpGet]
    [Route("metric/{id}")]
    public async Task<ActionResult<PostMetric>> GetMetricById(int id)
    {
      var metric = await _repo.GetPostMetricById(id);
      if (metric == null) return NotFound();
      return metric;
    }


    /// <summary>
    /// Search posts
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageRows"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<List<PostResponse>>> SearchPosts(int? offset, int? limit, [FromQuery] string[] tags)
    {
      var paging = new GetListParams<Post>(offset, limit);
      var posts = await _repo.SearchPosts(tags.ToList(), paging);
      return Ok(posts.Select(u => new PostResponse(u)).ToList());
    }


    /// <summary>
    /// Get a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PostResponse>> GetPost(int id)
    {
      var post = await _repo.GetPostById(id);
      if (post == null)
      {
        return NotFound("not-found");
      }
      return Ok(new PostResponse(post));
    }

    [HttpGet("{id}/increaseView")]
    public async Task<ActionResult> InscreaseView(int id)
    {
      try
      {
        var count = await _repo.IncreaseView(id);
        return Ok(new { count });
      }
      catch
      {
        return NotFound();
      }
    }

    /// <summary>
    /// Update a post
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PostResponse>> UpdatePostContent(int id, UpdatePostContent req)
    {
      var post = await _repo.UpdatePostContent(id, req.Title, req.Subtitle, req.CoverImageURL, req.Content, _auth.GetUser());
      return Ok(new PostResponse(post));
    }

    /// <summary>
    /// Update a post config
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}/config")]
    public async Task<ActionResult<PostResponse>> UpdatePostConfig(int id, UpdatePostConfig req)
    {
      var post = await _repo.UpdatePostConfig(id, req.PostRestrictionType, req.AccessUsers, req.CanComment);
      return Ok(new PostResponse(post));
    }

    /// <summary>
    /// Delete a post
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
      await _repo.DeletePost(id, _auth.GetUser());
      return Ok();
    }

    /// <summary>
    /// Add `tag` to a post which Id is postId
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{postId}/tag")]
    public async Task<ActionResult> AddTag(int postId, string tag)
    {
      var post = await _repo.GetPostById(postId, false);
      if (post == null)
        return NotFound();
      await _repo.AddTag(post, tag);
      return Ok();
    }

    /// <summary>
    /// Delete tag specified by postId and tag
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{postId}/tag")]
    public async Task<ActionResult> DeleteTag(int postId, string tag)
    {
      var post = await _repo.GetPostById(postId, false);
      if (post == null)
        return NotFound();
      await _repo.RemoveTag(post, tag);
      return Ok();
    }
  }
}
