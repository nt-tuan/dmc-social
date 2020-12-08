using Microsoft.EntityFrameworkCore.Migrations;

namespace DmcSocial.Migrations
{
  public partial class addpostsubtitleandcoverimages : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "Subject",
          table: "Posts",
          newName: "Title");

      migrationBuilder.AddColumn<string>(
          name: "CoverImageURL",
          table: "Posts",
          nullable: true);

      migrationBuilder.AddColumn<string>(
          name: "Subtitle",
          table: "Posts",
          nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CoverImageURL",
          table: "Posts");

      migrationBuilder.DropColumn(
          name: "Subtitle",
          table: "Posts");
      migrationBuilder.RenameColumn(
        name: "Title",
        table: "Posts",
        newName: "Subject");
      migrationBuilder.DropColumn(
          name: "Title",
          table: "Posts");
    }
  }
}
