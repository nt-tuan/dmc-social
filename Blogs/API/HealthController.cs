using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Interfaces;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Blogs.Entities;

namespace ThanhTuan.Blogs.API
{
  [Route("health")]
  [ApiController]
  public class HealthController : ControllerBase
  {
    [HttpGet]
    public ActionResult GetHealth()
    {
      return Ok(new { message = "OK" });
    }
  }
}