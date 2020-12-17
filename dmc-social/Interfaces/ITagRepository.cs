using System.Collections.Generic;
using System.Threading.Tasks;
using DmcSocial.Models;

namespace DmcSocial.Interfaces
{
  public interface ITagRepository
  {
    Task<List<Tag>> GetTags();
    Task<Tag> AddTag(Tag tag, string actor);
  }
}