using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Interfaces;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Entities;
using ThanhTuan.Blogs.Repositories;
using ThanhTuan.Blogs.API.Models;

namespace ThanhTuan.Blogs.API
{
  [Route("tag")]
  [ApiController]
  public class TagController : ControllerBase
  {
    private readonly ITagRepository _repo;
    private readonly Authenticate _auth;
    public TagController(ITagRepository repo, Authenticate auth)
    {
      _repo = repo;
      _auth = auth;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Tag>>> Get([FromQuery] PagingQuery query)
    {
      var entities = await _repo.GetTags(query.Limit, query.Offset);
      return Ok(entities.Select(u => new TagPayload(u)).ToList());
    }

    [HttpGet("batch")]
    public async Task<ActionResult<List<TagPayload>>> BatchGet([FromQuery] GetBatchTags query)
    {
      return (await _repo.BatchGetTags(query.Tags)).Select(e => new TagPayload(e)).ToList();
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Tag>> Put(string tag)
    {
      var user = _auth.GetUser();
      var e = new Tag(tag, user);
      await _repo.AddTag(e, _auth.GetUser());
      return Ok(e);
    }

    [HttpGet("related")]
    public async Task<ActionResult<List<string>>> GetRelatedTag([FromQuery] GetRelatedTagQuery query)
    {
      return await _repo.GetRelatedTags(query.Tag, query.Limit, query.Offset);
    }
  }
}