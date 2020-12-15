using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DmcSocial.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DmcSocial.Models;
using DmcSocial.Repositories;

namespace DmcSocial.API
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

    /// <summary>
    /// Get tags replated to `search`
    /// </summary>
    /// <param name="search"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Tag>>> Get(string search)
    {
      var entities = await _repo.GetTags(search);
      return Ok(entities.Select(u => new Models.Tag(u)).ToList());
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
      var user = _auth.GetUser();
      var e = new Tag(tag, user);
      await _repo.AddTag(e, _auth.GetUser());
      return Ok(e);
    }
  }
}