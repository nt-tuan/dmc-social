using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class addpostmetrics : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<int>(
          name: "ViewCount",
          table: "Posts",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "integer");

      migrationBuilder.AlterColumn<int>(
          name: "CommentCount",
          table: "Posts",
          nullable: true,
          oldClrType: typeof(int),
          oldType: "integer");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<int>(
          name: "ViewCount",
          table: "Posts",
          type: "integer",
          nullable: false,
          oldClrType: typeof(int),
          oldNullable: true);

      migrationBuilder.AlterColumn<int>(
          name: "CommentCount",
          table: "Posts",
          type: "integer",
          nullable: false,
          oldClrType: typeof(int),
          oldNullable: true);
    }
  }
}
