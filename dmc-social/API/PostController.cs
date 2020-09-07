using System.Net.Cache;
using System.Linq;
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
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPostRepository _repos;
        Authenticate _auth;
        public PostController(IPostRepository repos, Authenticate auth)
        {
            _repos = repos;
            _auth = auth;
        }

        /// <summary>
        /// Get posts
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRows"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PostResponse>>> GetPosts(int? pageIndex, int? pageRows, [FromQuery] string[] tags)
        {
            var paging = new GetListParams<Post>(pageIndex, pageRows);
            var posts = await _repos.GetPosts(tags.ToList(), paging);
            return Ok(posts.Select(u => new PostResponse(u)).ToList());
        }

        [HttpGet]
        [Route("count")]
        public async Task<ActionResult<int>> CountPosts(int? pageIndex, int? pageRows, [FromQuery] string[] tags)
        {
            var total = await _repos.CountPosts(tags.ToList());
            return total;
        }

        [HttpGet]
        [Route("metric/{id}")]
        public async Task<ActionResult<PostMetric>> GetMetricById(int id)
        {
            var metric = await _repos.GetPostMetricById(id);
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
        public async Task<ActionResult<List<PostResponse>>> SearchPosts(int? pageIndex, int? pageRows, [FromQuery] string[] tags, [FromQuery] string[] keywords)
        {
            var paging = new GetListParams<Post>(pageIndex, pageRows);
            var posts = await _repos.SearchPosts(tags.ToList(), paging);
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
            var post = await _repos.GetPostById(id);
            if (post == null)
            {
                return NotFound("not-found");
            }
            await _repos.MarkPostViewed(post);
            return Ok(new PostResponse(post));
        }

        /// <summary>
        /// Create a post
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<ActionResult<PostResponse>> CreatePost(CreatePost req)
        {
            var post = req.ToEntity();
            var postTags = post.PostTags;
            await _repos.CreatePost(post, _auth.GetUser());
            foreach (var postTag in postTags)
            {
                await _repos.AddTag(post, postTag.TagId);
            }
            return Ok(new PostResponse(post));
        }

        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<PostResponse>> UpdatePostContent(int id, UpdatePostContent req)
        {
            var post = await _repos.UpdatePostContent(id, req.subject, req.subject, _auth.GetUser());
            return Ok(new PostResponse(post));
        }

        /// <summary>
        /// Update a post config
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<PostResponse>> UpdatePostConfig(int id, UpdatePostContent req)
        {
            var post = await _repos.UpdatePostContent(id, req.subject, req.subject, _auth.GetUser());
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
            await _repos.DeletePost(id, _auth.GetUser());
            return Ok();
        }

        /// <summary>
        /// Add `tag` to a post which Id is postId
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{postId}/tag")]
        public async Task<ActionResult> AddTag(int postId, string tag)
        {
            var post = await _repos.GetPostById(postId, false);
            if (post == null)
                return NotFound();
            await _repos.AddTag(post, tag);
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
            var post = await _repos.GetPostById(postId, false);
            if (post == null)
                return NotFound();
            await _repos.RemoveTag(post, tag);
            return Ok();
        }
    }
}
