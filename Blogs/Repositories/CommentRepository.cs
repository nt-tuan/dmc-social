using System.Transactions;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThanhTuan.Blogs.Entities;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Blogs.Interfaces;
namespace ThanhTuan.Blogs.Repositories
{
  public class CommentRepository : ICommentRepository
  {
    private readonly AppDbContext _db;
    private readonly Repository _repo;
    public CommentRepository(AppDbContext db)
    {
      _db = db;
      _repo = new Repository(db);
    }

    private IQueryable<PostComment> GetQuery()
    {
      return _repo.GetQuery<PostComment>();
    }

    public async Task<List<PostComment>> GetSubPostComments(int commentId, ListParameter<PostComment> paging)
    {
      var query = GetQuery().Where(u => u.ParentPostCommentId == commentId);
      query = Helper.ApplyListParam(query, paging);
      var posts = await query.ToListAsync();
      return posts;
    }

    public async Task<int> GetSubPostCommentsCount(int commentId)
    {
      var query = GetQuery().Where(u => u.ParentPostCommentId == commentId);
      var count = await query.CountAsync();
      return count;
    }

    public async Task<List<PostComment>> GetPostComments(int postId, ListParameter<PostComment> paging)
    {
      var query = GetQuery().Where(u => u.PostId == postId && u.ParentPostCommentId == null);
      query = Helper.ApplyListParam(query, paging);
      var comments = await query.ToListAsync();
      return comments;
    }

    public async Task<int> GetPostCommentsCount(int postId)
    {
      var query = _db.PostComments.Where(u => u.PostId == postId && u.DateRemoved == null && u.ParentPostCommentId == null);
      var count = await query.CountAsync();
      return count;
    }

    public async Task<PostComment> CreatePostComment(int postId, int? commentId, string content, string by)
    {
      var comment = new PostComment
      {
        Content = content,
        PostId = postId,
        ParentPostCommentId = commentId,
        DateCreated = DateTimeOffset.Now,
        CreatedBy = by
      };
      _repo.Add(comment, by);
      await _db.SaveChangesAsync();
      await _db.Entry(comment).Reference(u => u.ParentPostComment).LoadAsync();
      if (comment.ParentPostComment != null)
        comment.ParentPostComment.CommentCount++;
      var post = await _db.Posts.FindAsync(comment.PostId);
      post.CommentCount++;
      _db.Posts.Attach(post).Property(u => u.CommentCount).IsModified = true;
      await _db.SaveChangesAsync();
      return comment;
    }

    public async Task<PostComment> UpdatePostComment(int id, string content, string by)
    {
      var comment = await GetPostCommentById(id);
      comment.Content = content;
      _db.Attach(comment).Property(comment => comment.Content).IsModified = true;
      _repo.Update(comment, by);
      await _db.SaveChangesAsync();
      return comment;
    }

    public async Task<PostComment> GetPostCommentById(int id)
    {
      var comment = await GetQuery().FirstOrDefaultAsync(u => u.Id == id);
      return comment;
    }

    public async Task DeleteComment(PostComment comment, string actor)
    {
      _repo.Delete(comment, actor);
      await _db.SaveChangesAsync();
      return;
    }
  }
}