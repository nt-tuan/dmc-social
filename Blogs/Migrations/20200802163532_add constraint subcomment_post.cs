using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class addconstraintsubcomment_post : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql(@"DELETE FROM ""PostComments"" where ""PostId"" is null");
      migrationBuilder.DropForeignKey(
          name: "FK_PostComments_Posts_PostId",
          table: "PostComments");

      migrationBuilder.AlterColumn<int>(
          name: "PostId",
          table: "PostComments",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "integer",
          oldNullable: true);

      migrationBuilder.AddColumn<int>(
          name: "CommentCount",
          table: "PostComments",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.AddForeignKey(
          name: "FK_PostComments_Posts_PostId",
          table: "PostComments",
          column: "PostId",
          principalTable: "Posts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_PostComments_Posts_PostId",
          table: "PostComments");

      migrationBuilder.DropColumn(
          name: "CommentCount",
          table: "PostComments");

      migrationBuilder.AlterColumn<int>(
          name: "PostId",
          table: "PostComments",
          type: "integer",
          nullable: true,
          oldClrType: typeof(int));

      migrationBuilder.AddForeignKey(
          name: "FK_PostComments_Posts_PostId",
          table: "PostComments",
          column: "PostId",
          principalTable: "Posts",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
