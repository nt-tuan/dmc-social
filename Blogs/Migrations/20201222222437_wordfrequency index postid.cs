using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class wordfrequencyindexpostid : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateIndex(
          name: "IX_WordFrequencies_PostId",
          table: "WordFrequencies",
          column: "PostId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "IX_WordFrequencies_PostId",
          table: "WordFrequencies");
    }
  }
}
