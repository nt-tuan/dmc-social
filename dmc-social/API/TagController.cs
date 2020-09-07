using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DmcSocial.Repositories;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;

namespace DmcSocial.API
{
  [Route("tag")]
  [ApiController]
  public class TagController : ControllerBase
  {
    ITagRepository _repos;
    public TagController(ITagRepository repos)
    {
      _repos = repos;
    }

    /// <summary>
    /// Get tags replated to `search`
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Tag>>> Get(string search)
    {
      var entities = await _repos.GetTags(search);
      return Ok(entities.Select(u => new Models.Tag { value = u.Value, postCount = u.PostCount, LastModifiedAt = u.LastModifiedTime }).ToList());
    }

    /// <summary>
    /// Create new tag
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Tag>> Put(string tag)
    {
      var e = new Tag(tag);
      await _repos.AddTag(e);
      return Ok(e);
    }
  }
}