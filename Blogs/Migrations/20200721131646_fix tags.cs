using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class fixtags : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags");

      migrationBuilder.DropPrimaryKey(
          name: "PK_PostTags",
          table: "PostTags");

      migrationBuilder.DropIndex(
          name: "IX_PostTags_PostId",
          table: "PostTags");

      migrationBuilder.DropColumn(
          name: "CreatedById",
          table: "Tags");

      migrationBuilder.DropColumn(
          name: "LastModifiedById",
          table: "Tags");

      migrationBuilder.DropColumn(
          name: "RemovedById",
          table: "Tags");

      migrationBuilder.DropColumn(
          name: "Id",
          table: "PostTags");

      migrationBuilder.DropColumn(
          name: "CreatedById",
          table: "PostTags");

      migrationBuilder.DropColumn(
          name: "LastModifiedById",
          table: "PostTags");

      migrationBuilder.DropColumn(
          name: "RemovedById",
          table: "PostTags");

      migrationBuilder.DropColumn(
          name: "CreatedById",
          table: "Posts");

      migrationBuilder.DropColumn(
          name: "LastModifiedById",
          table: "Posts");

      migrationBuilder.DropColumn(
          name: "RemovedById",
          table: "Posts");

      migrationBuilder.DropColumn(
          name: "CreatedById",
          table: "PostComments");

      migrationBuilder.DropColumn(
          name: "LastModifiedById",
          table: "PostComments");

      migrationBuilder.DropColumn(
          name: "RemovedById",
          table: "PostComments");

      migrationBuilder.AlterColumn<string>(
          name: "TagId",
          table: "PostTags",
          nullable: false,
          oldClrType: typeof(string),
          oldType: "text",
          oldNullable: true);

      migrationBuilder.AddPrimaryKey(
          name: "PK_PostTags",
          table: "PostTags",
          columns: new[] { "PostId", "TagId" });

      migrationBuilder.AddForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags",
          column: "TagId",
          principalTable: "Tags",
          principalColumn: "Value",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags");

      migrationBuilder.DropPrimaryKey(
          name: "PK_PostTags",
          table: "PostTags");

      migrationBuilder.AddColumn<string>(
          name: "CreatedById",
          table: "Tags",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "LastModifiedById",
          table: "Tags",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "RemovedById",
          table: "Tags",
          type: "text",
          nullable: true);

      migrationBuilder.AlterColumn<string>(
          name: "TagId",
          table: "PostTags",
          type: "text",
          nullable: true,
          oldClrType: typeof(string));

      migrationBuilder.AddColumn<int>(
          name: "Id",
          table: "PostTags",
          type: "integer",
          nullable: false,
          defaultValue: 0)
          .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

      migrationBuilder.AddColumn<string>(
          name: "CreatedById",
          table: "PostTags",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "LastModifiedById",
          table: "PostTags",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "RemovedById",
          table: "PostTags",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "CreatedById",
          table: "Posts",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "LastModifiedById",
          table: "Posts",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "RemovedById",
          table: "Posts",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "CreatedById",
          table: "PostComments",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "LastModifiedById",
          table: "PostComments",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "RemovedById",
          table: "PostComments",
          type: "text",
          nullable: true);

      migrationBuilder.AddPrimaryKey(
          name: "PK_PostTags",
          table: "PostTags",
          column: "Id");

      migrationBuilder.CreateIndex(
          name: "IX_PostTags_PostId",
          table: "PostTags",
          column: "PostId");

      migrationBuilder.AddForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags",
          column: "TagId",
          principalTable: "Tags",
          principalColumn: "Value",
          onDelete: ReferentialAction.Restrict);
    }
  }
}
