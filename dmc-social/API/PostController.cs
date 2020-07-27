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
        public PostController(IPostRepository repos)
        {
            _repos = repos;
        }

        /// <summary>
        /// Get posts
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRows"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
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


        /// <summary>
        /// Search posts
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageRows"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<PostResponse>>> SearchPosts(int? pageIndex, int? pageRows, [FromQuery] string[] tags)
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
            post.CreatedBy = getUser();
            await _repos.CreatePost(post);
            return Ok(new PostResponse(post));
        }

        /// <summary>
        /// Update a post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<ActionResult<PostResponse>> UpdatePost(int id, UpdatePost req)
        {
            var post = await _repos.GetPostById(id, false);
            if (post == null)
            {
                return NotFound();
            }
            post.Subject = req.subject;
            post.Content = req.content;
            post.CanComment = req.canComment;
            post.PostRestrictionType = req.postRestrictionType;
            if (req.accessUsers != null)
            {
                post.PostAccessUsers = req.accessUsers;
            }
            post.LastModifiedBy = getUser();
            await _repos.UpdatePost(post);
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
            var post = await _repos.GetPostById(id);
            if (post == null)
            {
                return NotFound();
            }
            await _repos.DeletePost(post);
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

        private string getUser()
        {
            return "tuannt";
        }
    }
}
