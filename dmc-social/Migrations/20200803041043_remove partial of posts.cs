using Microsoft.EntityFrameworkCore.Migrations;

namespace DmcSocial.Migrations
{
    public partial class removepartialofposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ViewCount",
                table: "Posts",
                newName: "Metric_ViewCount");

            migrationBuilder.RenameColumn(
                name: "CommentCount",
                table: "Posts",
                newName: "Metric_CommentCount");

            migrationBuilder.RenameColumn(
                name: "PostRestrictionType",
                table: "Posts",
                newName: "Config_PostRestrictionType");

            migrationBuilder.RenameColumn(
                name: "PostAccessUsers",
                table: "Posts",
                newName: "Config_PostAccessUsers");

            migrationBuilder.AddColumn<int>(
                name: "Config_Id",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Metric_Id",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Config_Id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Metric_Id",
                table: "Posts");

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
        }
    }
}
