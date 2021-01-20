using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Interfaces;
using System.Collections.Generic;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.API.Models;
using ThanhTuan.Blogs.Repositories;
using Microsoft.Extensions.Logging;

namespace ThanhTuan.Blogs.API
{
  [Route("post")]
  [ApiController]
  public class PostController : ControllerBase
  {
    private readonly IPostRepository _repo;
    private readonly Authenticate _auth;
    private readonly ILogger<PostController> _logger;
    public PostController(IPostRepository repo, Authenticate auth, ILogger<PostController> logger)
    {
      _logger = logger;
      _repo = repo;
      _auth = auth;
    }

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
    [Route("count/byAuthor")]
    public async Task<ActionResult<List<GroupByAuthor>>> GetTopBloggers([FromQuery] PagingQuery query)
    {
      var parameter = new PagingParameter<GroupByAuthor>
      {
        Offset = query.Offset,
        Limit = query.Limit,
        OrderDirection = (PagingParameter<GroupByAuthor>.OrderDirections)(query.Dir ?? 1),
        OrderBy = group => group.TotalPost
      };
      var groups = await _repo.GetPostsGroupByAuthor(parameter);
      return groups;
    }

    [HttpGet]
    [Route("metric/{id}")]
    public async Task<ActionResult<PostMetric>> GetMetricById(int id)
    {
      var metric = await _repo.GetPostMetricById(id);
      if (metric == null) return NotFound();
      return metric;
    }

    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<List<PostResponse>>> SearchPosts([FromQuery] SearchPostQuery query)
    {
      var posts = await _repo.SearchPosts(query.Tags?.ToList(), query.Keywords?.ToList(), query.Offset, query.Limit);
      return Ok(posts.Select(u => new PostResponse(u)).ToList());
    }

    [HttpGet]
    [Route("search/count")]
    public async Task<ActionResult<int>> CountSearchPosts([FromQuery] string[] tags, [FromQuery] string[] keywords)
    {
      var count = await _repo.CountSearchedPosts(tags.ToList(), keywords.ToList());
      return Ok(count);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostResponse>> GetPost(int id)
    {
      var post = await _repo.GetPostById(id);
      if (post == null)
      {
        return NotFound();
      }
      return new PostResponse(post); ;
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

    [HttpPut("{id}")]
    public async Task<ActionResult<PostResponse>> UpdatePostContent(int id, UpdatePostContent req)
    {
      var post = await _repo.UpdatePostContent(id, req.Title, req.Subtitle, req.CoverImageURL, req.Content, _auth.GetUser());
      return Ok(new PostResponse(post));
    }

    [HttpPut("{id}/config")]
    public async Task<ActionResult<PostResponse>> UpdatePostConfig(int id, UpdatePostConfig req)
    {
      var post = await _repo.UpdatePostConfig(id, req.PostRestrictionType, req.AccessUsers, req.CanComment);
      return Ok(new PostResponse(post));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
      await _repo.DeletePost(id, _auth.GetUser());
      return Ok();
    }

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
