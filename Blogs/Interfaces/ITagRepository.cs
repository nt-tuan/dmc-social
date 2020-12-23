using System.Collections.Generic;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Entities;

namespace ThanhTuan.Blogs.Interfaces
{
  public interface ITagRepository
  {
    Task<List<Tag>> GetTags(string search, int limit, int offset);
    Task<List<Tag>> BatchGetTags(List<string> tags);
    Task<Tag> AddTag(Tag tag, string actor);

    Task<List<string>> GetRelatedTags(string tag, int limit, int offset);
  }
}