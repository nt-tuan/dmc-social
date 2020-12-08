using Microsoft.EntityFrameworkCore;
using DmcSocial.Models;

namespace DmcSocial.Repositories
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    { }

    public DbSet<Post> Posts { get; set; }
    public DbSet<PostComment> PostComments { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      builder.Entity<PostComment>().HasOne(u => u.Post).WithMany(u => u.Comments).HasForeignKey(u => u.PostId);
      builder.Entity<PostComment>().HasOne(u => u.ParentPostComment).WithMany(u => u.ChildrenPostComments).HasForeignKey(u => u.ParentPostCommentId);
      builder.Entity<Tag>().HasKey(u => u.Value);
      builder.Entity<PostTag>().HasKey(u => new { u.PostId, u.TagId });
      builder.Entity<PostTag>().HasOne(u => u.Post).WithMany(u => u.PostTags).HasForeignKey(u => u.PostId);
      builder.Entity<PostTag>().HasOne(u => u.Tag).WithMany().HasForeignKey(u => u.TagId);
    }

  }
}