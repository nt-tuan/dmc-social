using Microsoft.EntityFrameworkCore.Migrations;

namespace DmcSocial.Migrations
{
    public partial class fixmetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Config_Id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Metric_Id",
                table: "Posts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Config_Id",
                table: "Posts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Metric_Id",
                table: "Posts",
                type: "integer",
                nullable: true);
        }
    }
}
