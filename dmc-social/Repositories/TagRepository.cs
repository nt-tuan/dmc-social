using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Models;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _db;
        public TagRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Tag> AddTag(Tag tag)
        {
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
            return tag;
        }

        public async Task<List<Tag>> GetTags(string search)
        {
            var query = _db.Tags
            .Where(u => u.DateRemoved == null);
            var normalizeValue = Helper.NormalizeString(search);
            if (!string.IsNullOrEmpty(search))
                query = query.Where(u => search == "" || u.NormalizeValue.Contains(search));
            var tags = await query.ToListAsync();
            return tags;
        }
    }
}