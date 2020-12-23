using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThanhTuan.Blogs.Migrations
{
  public partial class updatewordfrequency : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "CreatedBy",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "DateCreated",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "DateRemoved",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "Id",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "LastModifiedBy",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "LastModifiedTime",
          table: "WordFrequencies");

      migrationBuilder.DropColumn(
          name: "RemovedBy",
          table: "WordFrequencies");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "CreatedBy",
          table: "WordFrequencies",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "DateCreated",
          table: "WordFrequencies",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "DateRemoved",
          table: "WordFrequencies",
          type: "timestamp with time zone",
          nullable: true);

      migrationBuilder.AddColumn<int>(
          name: "Id",
          table: "WordFrequencies",
          type: "integer",
          nullable: false,
          defaultValue: 0);

      migrationBuilder.AddColumn<string>(
          name: "LastModifiedBy",
          table: "WordFrequencies",
          type: "text",
          nullable: true);

      migrationBuilder.AddColumn<DateTimeOffset>(
          name: "LastModifiedTime",
          table: "WordFrequencies",
          type: "timestamp with time zone",
          nullable: false,
          defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

      migrationBuilder.AddColumn<string>(
          name: "RemovedBy",
          table: "WordFrequencies",
          type: "text",
          nullable: true);
    }
  }
}
