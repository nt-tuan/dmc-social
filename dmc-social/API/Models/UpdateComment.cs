using System.ComponentModel.DataAnnotations;

namespace DmcSocial.API.Models
{
  public class UpdateComment
  {
    [MinLength(8)]
    [MaxLength(1024)]
    public string Content { get; set; }
  }
}