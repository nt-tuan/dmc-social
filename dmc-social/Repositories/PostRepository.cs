using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Runtime.ExceptionServices;
using System.Diagnostics.Tracing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Models;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _db;
        public PostRepository(AppDbContext db)
        {
            _db = db;
        }

        private IQueryable<Post> getPostQuery()
        {
            return _db.Posts.Include(u => u.PostTags).AsQueryable();
        }

        public async Task<Post> CreatePost(Post post)
        {
            _db.Add(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<Post> GetPostById(int id, bool load = true)
        {
            var query = _db.Posts.AsQueryable();
            if (load)
            {
                query = getPostQuery();
            }
            var post = await query.Where(post => post.Id == id && post.DateRemoved == null).FirstOrDefaultAsync();
            return post;
        }

        private IQueryable<Post> getPostsByTagsQuery(List<string> tags)
        {
            var query = getPostQuery();
            var tagsCount = tags.Count();
            if (tags.Count > 0)
            {
                query = query
                .Where(u => u.DateRemoved == null)
                .Where(u => u.PostTags.Where(u => tags.Contains(u.TagId)).Count() >= tagsCount);
            }
            return query;
        }

        public async Task<List<Post>> GetPosts(List<string> tags, GetListParams<Post> paging)
        {
            var query = getPostsByTagsQuery(tags);
            query = Helper.ApplyPaging(query, paging);
            var posts = await query.ToListAsync();
            return posts;
        }

        public async Task<int> CountPosts(List<string> tags)
        {
            var query = getPostsByTagsQuery(tags);
            var count = await query.CountAsync();
            return count;
        }

        public async Task UpdatePost(Post entity)
        {
            if (entity == null)
                throw PostException.PostNotFound;
            // Ignore update posttags
            entity.PostTags = null;
            _db.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeletePost(Post entity)
        {
            if (entity == null || entity.DateRemoved != null)
                throw PostException.PostNotFound;
            _db.Posts.Remove(entity);
            await _db.SaveChangesAsync();
        }

        private async Task<Tag> getTag(string tag)
        {
            var entity = await _db.Tags.FindAsync(tag);
            return entity;
        }
        public async Task AddTag(Post post, string tag)
        {
            var tagEntities = await getTag(tag);
            if (tagEntities == null)
                throw TagException.TagNotFound;

            var entity = new PostTag { PostId = post.Id, TagId = tag };
            _db.PostTags.Add(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveTag(Post post, string tag)
        {
            var entity = await _db.PostTags.FindAsync(post.Id, tag);
            if (entity == null)
                throw TagException.TagNotFound;
            _db.PostTags.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Post>> SearchPosts(List<string> tagIds, GetListParams<Post> param)
        {
            if (tagIds.Count() == 0)
                return new List<Post>();
            var relatedPostTags = await _db.PostTags
            .Where(u => tagIds.Contains(u.TagId) && u.Post.DateRemoved == null)
            .GroupBy(u => u.PostId)
            .Select(u => new
            {
                PostId = u.Key,
                Count = u.Count()
            })
            .Skip(param.GetSkipNumber())
            .Take(param.pageRows)
            .OrderByDescending(u => u.Count)
            .ToListAsync();

            var postIds = relatedPostTags.Select(u => u.PostId);
            if (postIds.Count() == 0)
            {
                return new List<Post>();
            }
            var posts = await _db.Posts.Where(u => postIds.Contains(u.Id)).ToListAsync();

            posts.Sort((a, b) =>
            {
                var countA = postIds.FirstOrDefault(u => u == a.Id);
                var countB = postIds.FirstOrDefault(u => u == b.Id);
                return countA.CompareTo(countB);
            });
            return posts;
        }
    }
}
