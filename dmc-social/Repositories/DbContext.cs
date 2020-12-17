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
    public DbSet<WordFrequency> WordFrequencies { get; set; }
    public DbSet<TagCorrelationCoefficient> TagCorrelationCoefficients { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Post
      Post.OnModelCreating<Post>(builder);
      builder.Entity<Post>().HasIndex(u => u.Popularity);

      // PostComment
      Post.OnModelCreating<PostComment>(builder);
      builder.Entity<PostComment>().HasOne(u => u.Post).WithMany(u => u.Comments).HasForeignKey(u => u.PostId);
      builder.Entity<PostComment>().HasOne(u => u.ParentPostComment).WithMany(u => u.ChildrenPostComments).HasForeignKey(u => u.ParentPostCommentId);

      // Tag
      Tag.OnModelCreating<Tag>(builder);
      builder.Entity<Tag>().Ignore(u => u.Id);
      builder.Entity<Tag>().HasKey(u => u.Slug);
      builder.Entity<Tag>().HasIndex(u => u.Popularity);

      // PostTag
      PostTag.OnModelCreating<PostTag>(builder);
      builder.Entity<PostTag>().HasKey(u => new { u.PostId, u.TagId });
      builder.Entity<PostTag>().HasOne(u => u.Post).WithMany(u => u.PostTags).HasForeignKey(u => u.PostId);
      builder.Entity<PostTag>().HasOne(u => u.Tag).WithMany().HasForeignKey(u => u.TagId);

      // WordFrequency
      builder.Entity<WordFrequency>().HasKey(u => new { u.Word, u.PostId });

      // TagCorrelationCoefficient
      builder.Entity<TagCorrelationCoefficient>().HasKey(u => new { u.TagX, u.TagY });
      builder.Entity<TagCorrelationCoefficient>().Property(u => u.Coefficient).HasColumnType("decimal(16,2)");
    }
  }
}