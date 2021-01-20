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
    public async Task<ActionResult<List<Tag>>> Get([FromQuery] GetTagsQuery query)
    {
      var entities = await _repo.GetTags(query.Search, query.Limit, query.Offset);
      return Ok(entities.Select(u => new TagPayload(u)).ToList());
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count()
    {
      return await _repo.CountTags();
    }


    [HttpGet("batch")]
    public async Task<ActionResult<List<TagPayload>>> BatchGet([FromQuery] GetBatchTags query)
    {
      return (await _repo.BatchGetTags(query.Tags)).Select(e => new TagPayload(e)).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<TagPayload>> Put(string tag)
    {
      var user = _auth.GetUser();
      var e = new Tag(tag, user);
      await _repo.AddTag(e, _auth.GetUser());
      return Ok(new TagPayload(e));
    }

    [HttpGet("related")]
    public async Task<ActionResult<List<string>>> GetRelatedTag([FromQuery] GetRelatedTagQuery query)
    {
      return await _repo.GetRelatedTags(query.Tag, query.Limit, query.Offset);
    }

    [HttpPut]
    public async Task<ActionResult<TagPayload>> UpdateTagValue(UpdateTagPayload payload)
    {
      var entity = await _repo.UpdateTag(payload.Slug, payload.Label, _auth.GetUser());
      return new TagPayload(entity);
    }

    [HttpDelete("{tag}")]
    public async Task<ActionResult> DeleteTag(string tag)
    {
      await _repo.DeleteTag(tag, _auth.GetUser());
      return Ok();
    }
  }
}