using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ThanhTuan.Blogs.API.Models
{
  public class PagingQuery
  {
    [FromQuery]
    public int Offset { get; set; }
    [FromQuery]
    [Range(0, 100)]
    public int Limit { get; set; }
  }
}