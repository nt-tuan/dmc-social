using System.Collections.Generic;
using System.Threading.Tasks;
using DmcSocial.Models;

namespace DmcSocial.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetTags(string search);
        Task<Tag> AddTag(Tag tag);
    }
}