using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class fixownedentity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "Metric_ViewCount",
          table: "Posts",
          newName: "ViewCount");

      migrationBuilder.RenameColumn(
          name: "Metric_CommentCount",
          table: "Posts",
          newName: "CommentCount");

      migrationBuilder.RenameColumn(
          name: "Config_PostRestrictionType",
          table: "Posts",
          newName: "PostRestrictionType");

      migrationBuilder.RenameColumn(
          name: "Config_PostAccessUsers",
          table: "Posts",
          newName: "PostAccessUsers");

      migrationBuilder.AlterColumn<int>(
          name: "ViewCount",
          table: "Posts",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer",
          oldNullable: true);

      migrationBuilder.AlterColumn<int>(
          name: "CommentCount",
          table: "Posts",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer",
          oldNullable: true);

      migrationBuilder.AlterColumn<int>(
          name: "PostRestrictionType",
          table: "Posts",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer",
          oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "ViewCount",
          table: "Posts",
          newName: "Metric_ViewCount");

      migrationBuilder.RenameColumn(
          name: "PostRestrictionType",
          table: "Posts",
          newName: "Config_PostRestrictionType");

      migrationBuilder.RenameColumn(
          name: "PostAccessUsers",
          table: "Posts",
          newName: "Config_PostAccessUsers");

      migrationBuilder.RenameColumn(
          name: "CommentCount",
          table: "Posts",
          newName: "Metric_CommentCount");

      migrationBuilder.AlterColumn<int>(
          name: "Metric_ViewCount",
          table: "Posts",
          type: "integer",
          nullable: true,
          oldClrType: typeof(int));

      migrationBuilder.AlterColumn<int>(
          name: "Config_PostRestrictionType",
          table: "Posts",
          type: "integer",
          nullable: true,
          oldClrType: typeof(int));

      migrationBuilder.AlterColumn<int>(
          name: "Metric_CommentCount",
          table: "Posts",
          type: "integer",
          nullable: true,
          oldClrType: typeof(int));
    }
  }
}
