using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class IndexingSchedule : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "IndexingSchedules",
          columns: table => new
          {
            Id = table.Column<int>(nullable: false)
                  .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            StartAt = table.Column<DateTimeOffset>(nullable: false),
            EndAt = table.Column<DateTimeOffset>(nullable: false),
            RunAt = table.Column<DateTimeOffset>(nullable: false),
            State = table.Column<string>(nullable: true),
            Message = table.Column<string>(nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_IndexingSchedules", x => x.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "IndexingSchedules");
    }
  }
}
