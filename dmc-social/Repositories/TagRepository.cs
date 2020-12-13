using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Interfaces;
using DmcSocial.Models;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Repositories
{
  public class TagRepository : ITagRepository
  {
    private readonly AppDbContext _db;
    private readonly Repository _repo;
    public TagRepository(AppDbContext db)
    {
      _db = db;
      _repo = new Repository(db);
    }

    public async Task<Tag> AddTag(Tag tag, string actor)
    {
      _repo.Add(tag, actor);
      await _db.SaveChangesAsync();
      return tag;
    }

    public async Task<List<Tag>> GetTags(string search)
    {
      var query = _repo.GetQuery<Tag>();
      var normalizeValue = Helper.NormalizeTag(search);
      if (!string.IsNullOrEmpty(search))
        query = query.Where(u => search == "" || u.NormalizeValue.Contains(normalizeValue));
      var tags = await query.ToListAsync();
      return tags;
    }
  }
}