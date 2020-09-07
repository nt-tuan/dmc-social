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