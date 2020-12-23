using System.Net.Cache;
using System.Linq;
using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Interfaces;
using System.Collections.Generic;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.API.Models;
using ThanhTuan.Blogs.Repositories;
using System.Linq.Expressions;

namespace ThanhTuan.Blogs.API
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
    public async Task<ActionResult<List<PostResponse>>> GetPosts([FromQuery] GetPostQuery query)
    {
      var parameter = query.ToParameter();
      var posts = await _repo.GetPosts(parameter);
      return Ok(posts.Select(u => new PostResponse(u)).ToList());
    }

    [HttpGet]
    [Route("count")]
    public async Task<ActionResult<int>> CountPosts([FromQuery] PostFilterQuery query)
    {
      var total = await _repo.CountPosts(query.ToParameter());
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
    public async Task<ActionResult<List<PostResponse>>> SearchPosts([FromQuery] SearchPostQuery query)
    {
      var posts = await _repo.SearchPosts(query.Tags?.ToList(), query.Keywords?.ToList(), query.Offset, query.Limit);
      return Ok(posts.Select(u => new PostResponse(u)).ToList());
    }

    /// <summary>
    /// Count search result posts
    /// </summary>
    /// <param name="pageIndex"></param>
    /// <param name="pageRows"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("search/count")]
    public async Task<ActionResult<int>> CountSearchPosts([FromQuery] string[] tags, [FromQuery] string[] keywords)
    {
      var count = await _repo.CountSearchedPosts(tags.ToList(), keywords.ToList());
      return Ok(count);
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

    [HttpGet("me")]
    public async Task<ActionResult<List<PostResponse>>> GetMyPost([FromQuery] GetPostQuery query)
    {
      var user = _auth.GetUser();
      if (string.IsNullOrEmpty(user)) return Unauthorized();
      var getParams = query.ToParameter();
      getParams.Filter.FilterQueries.Add(post => post.CreatedBy == user);
      var posts = await _repo.GetPosts(getParams);
      return posts.Select(post => new PostResponse(post)).ToList();
    }

    [HttpPost("me/count")]
    public async Task<ActionResult<int>> GetMyPostCount([FromQuery] PostFilterQuery query)
    {
      var parameter = query.ToParameter();
      parameter.Filter.FilterQueries.Add(post => post.CreatedBy == _auth.GetUser());
      return await _repo.CountPosts(parameter);
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
      await _repo.AddTag(post, tag, _auth.GetUser());
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
      await _repo.RemoveTag(post, tag, _auth.GetUser());
      return Ok();
    }
  }
}
