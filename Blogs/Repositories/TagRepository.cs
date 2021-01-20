using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Interfaces;
using ThanhTuan.Blogs.Entities;
using Microsoft.EntityFrameworkCore;

namespace ThanhTuan.Blogs.Repositories
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

    public async Task<List<Tag>> GetTags(string search, int limit, int offset)
    {
      string normalizedSearchValue = null;
      if (!string.IsNullOrEmpty(search)) normalizedSearchValue = Helper.NormalizeTag(search);
      var tags = await _repo.GetQuery<Tag>().Where(tag => string.IsNullOrEmpty(normalizedSearchValue) || tag.Slug.Contains(normalizedSearchValue)).OrderByDescending(u => u.Popularity).Skip(offset).Take(limit).ToListAsync();
      return tags;
    }

    public async Task<int> CountTags()
    {
      return await _repo.GetQuery<Tag>().CountAsync();
    }

    public async Task<List<Tag>> BatchGetTags(List<string> tags)
    {
      var entities = await _repo.GetQuery<Tag>().Where(e => tags.Contains(e.Slug)).ToListAsync();
      return entities;
    }

    public async Task<List<string>> GetRelatedTags(string tag, int limit, int offset)
    {
      var tags = await _db.TagCorrelationCoefficients.Where(e => e.TagY == tag).OrderByDescending(e => e.Coefficient).Skip(offset).Take(limit).Select(e => e.TagX).ToListAsync();
      return tags;
    }

    public async Task<Tag> UpdateTag(string slug, string label, string actor)
    {
      var tag = await _db.Tags.FirstOrDefaultAsync(u => u.Slug == slug);
      tag.Value = label;
      _db.Entry(tag).Property(u => u.Value).IsModified = true;
      _repo.Update(tag, actor);
      await _db.SaveChangesAsync();
      return tag;
    }

    public async Task DeleteTag(string slug, string actor)
    {
      var hasPosts = await _db.PostTags.AnyAsync();
      if (hasPosts)
      {
        throw new System.Exception("has-posts-attached");
      }
      var tag = await _repo.GetQuery<Tag>().FirstOrDefaultAsync(u => u.Slug == slug);
      _repo.Delete(tag, actor);
      await _db.SaveChangesAsync();
    }
  }
}