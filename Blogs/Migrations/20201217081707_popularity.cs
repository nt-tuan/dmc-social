using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class popularity : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Tags",
          table: "Tags");

      migrationBuilder.DropIndex(
          name: "IX_Posts_LookupValue",
          table: "Posts");

      migrationBuilder.RenameColumn(
          name: "NormalizeValue",
          table: "Tags",
          newName: "Slug");

      migrationBuilder.DropColumn(
          name: "LookupValue",
          table: "Posts");

      migrationBuilder.AddColumn<decimal>(
          name: "Popularity",
          table: "Tags",
          nullable: false,
          defaultValue: 0m);

      migrationBuilder.AddColumn<decimal>(
          name: "Popularity",
          table: "Posts",
          nullable: false,
          defaultValue: 0m);

      migrationBuilder.AddPrimaryKey(
          name: "PK_Tags",
          table: "Tags",
          column: "Slug");

      migrationBuilder.CreateTable(
          name: "TagCorrelationCoefficients",
          columns: table => new
          {
            TagX = table.Column<string>(nullable: false),
            TagY = table.Column<string>(nullable: false),
            Coefficient = table.Column<decimal>(type: "decimal(16,2)", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_TagCorrelationCoefficients", x => new { x.TagX, x.TagY });
          });

      migrationBuilder.CreateIndex(
          name: "IX_Tags_Popularity",
          table: "Tags",
          column: "Popularity");

      migrationBuilder.CreateIndex(
          name: "IX_Posts_Popularity",
          table: "Posts",
          column: "Popularity");

      migrationBuilder.AddForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags",
          column: "TagId",
          principalTable: "Tags",
          principalColumn: "Slug",
          onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags");

      migrationBuilder.DropTable(
          name: "TagCorrelationCoefficients");

      migrationBuilder.DropPrimaryKey(
          name: "PK_Tags",
          table: "Tags");

      migrationBuilder.DropIndex(
          name: "IX_Tags_Popularity",
          table: "Tags");

      migrationBuilder.DropIndex(
          name: "IX_Posts_Popularity",
          table: "Posts");

      migrationBuilder.RenameColumn(
          name: "Slug",
          table: "Tags",
          newName: "NormalizeValue");

      migrationBuilder.DropColumn(
          name: "Popularity",
          table: "Tags");

      migrationBuilder.DropColumn(
          name: "Popularity",
          table: "Posts");

      migrationBuilder.AddColumn<string>(
          name: "LookupValue",
          table: "Posts",
          type: "text",
          nullable: true);

      migrationBuilder.AddPrimaryKey(
          name: "PK_Tags",
          table: "Tags",
          column: "NormalizeValue");

      migrationBuilder.CreateIndex(
          name: "IX_Posts_LookupValue",
          table: "Posts",
          column: "LookupValue");

      migrationBuilder.AddForeignKey(
          name: "FK_PostTags_Tags_TagId",
          table: "PostTags",
          column: "TagId",
          principalTable: "Tags",
          principalColumn: "NormalizeValue",
          onDelete: ReferentialAction.Cascade);
    }
  }
}
