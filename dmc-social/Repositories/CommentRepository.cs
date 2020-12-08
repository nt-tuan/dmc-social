using System.Transactions;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DmcSocial.Models;
using Microsoft.EntityFrameworkCore;
using DmcSocial.Interfaces;
namespace DmcSocial.Repositories
{
  public class CommentRepository : ICommentRepository
  {
    private readonly AppDbContext _db;
    public CommentRepository(AppDbContext db)
    {
      _db = db;
    }

    public async Task<List<PostComment>> GetSubPostComments(int commentId, GetListParams<PostComment> paging)
    {
      var query = _db.PostComments.Where(u => u.ParentPostCommentId == commentId && u.DateRemoved == null);
      query = Helper.ApplyPaging(query, paging);
      var posts = await query.ToListAsync();
      return posts;
    }

    public async Task<int> GetSubPostCommentsCount(int commentId)
    {
      var query = _db.PostComments.Where(u => u.ParentPostCommentId == commentId && u.DateRemoved == null);
      var count = await query.CountAsync();
      return count;
    }

    public async Task<List<PostComment>> GetPostComments(int postId, GetListParams<PostComment> paging)
    {
      var query = _db.PostComments.Where(u => u.PostId == postId && u.DateRemoved == null && u.ParentPostCommentId == null);
      query = Helper.ApplyPaging(query, paging);
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
        DateCreated = DateTime.Now,
        CreatedBy = by
      };
      _db.PostComments.Add(comment);
      await _db.SaveChangesAsync();
      await _db.Entry(comment).Reference(u => u.ParentPostComment).LoadAsync();
      if (comment.ParentPostComment != null)
        comment.ParentPostComment.CommentCount++;
      var post = await _db.Posts.FindAsync(comment.PostId);
      post.CommentCount++;
      _db.Posts.Update(post);
      await _db.SaveChangesAsync();
      return comment;
    }

    public async Task<PostComment> UpdatePostComment(int id, string content, string by)
    {
      var comment = await _db.PostComments.FindAsync(id);
      comment.Content = content;
      comment.LastModifiedTime = DateTime.Now;
      comment.LastModifiedBy = by;
      _db.Attach(comment).Property(comment => new
      {
        comment.Content,
        comment.LastModifiedTime,
        comment.LastModifiedBy
      })
      .IsModified = true;
      await _db.SaveChangesAsync();
      return comment;
    }

    public async Task<PostComment> GetPostCommentById(int id)
    {
      var comment = await _db.PostComments.FindAsync(id);
      return comment;
    }

    public async Task DeleteComment(PostComment comment)
    {
      _db.PostComments.Remove(comment);
      await _db.SaveChangesAsync();
      return;
    }
  }
}