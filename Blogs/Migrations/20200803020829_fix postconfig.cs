using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class fixpostconfig : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "Config_PostRestrictionType",
          table: "Posts",
          newName: "PostRestrictionType");

      migrationBuilder.RenameColumn(
          name: "Config_PostAccessUsers",
          table: "Posts",
          newName: "PostAccessUsers");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "PostRestrictionType",
          table: "Posts",
          newName: "Config_PostRestrictionType");

      migrationBuilder.RenameColumn(
          name: "PostAccessUsers",
          table: "Posts",
          newName: "Config_PostAccessUsers");
    }
  }
}
