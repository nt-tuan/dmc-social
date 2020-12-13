using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DmcSocial.Migrations
{
    public partial class wf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsSystemTag",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeValue",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateRemoved",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "Tags",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Tags",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Tags",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "PostTags",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateRemoved",
                table: "PostTags",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "PostTags",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PostTags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateRemoved",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<string>(
                name: "LookupValue",
                table: "Posts",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastModifiedTime",
                table: "PostComments",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateRemoved",
                table: "PostComments",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "PostComments",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "NormalizeValue");

            migrationBuilder.CreateTable(
                name: "WordFrequencies",
                columns: table => new
                {
                    Word = table.Column<string>(nullable: false),
                    PostId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    DateRemoved = table.Column<DateTimeOffset>(nullable: true),
                    RemovedBy = table.Column<string>(nullable: true),
                    LastModifiedTime = table.Column<DateTimeOffset>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    Frequency = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordFrequencies", x => new { x.Word, x.PostId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_DateRemoved",
                table: "Tags",
                column: "DateRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_LastModifiedTime",
                table: "Tags",
                column: "LastModifiedTime");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_DateRemoved",
                table: "PostTags",
                column: "DateRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_LastModifiedTime",
                table: "PostTags",
                column: "LastModifiedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DateRemoved",
                table: "Posts",
                column: "DateRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LastModifiedTime",
                table: "Posts",
                column: "LastModifiedTime");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_LookupValue",
                table: "Posts",
                column: "LookupValue");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_DateRemoved",
                table: "PostComments",
                column: "DateRemoved");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_LastModifiedTime",
                table: "PostComments",
                column: "LastModifiedTime");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "NormalizeValue",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags");

            migrationBuilder.DropTable(
                name: "WordFrequencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_DateRemoved",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_LastModifiedTime",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_DateRemoved",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_LastModifiedTime",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_Posts_DateRemoved",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LastModifiedTime",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_LookupValue",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_DateRemoved",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_LastModifiedTime",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "LookupValue",
                table: "Posts");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Tags",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedTime",
                table: "Tags",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRemoved",
                table: "Tags",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Tags",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<string>(
                name: "NormalizeValue",
                table: "Tags",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemTag",
                table: "Tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedTime",
                table: "PostTags",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRemoved",
                table: "PostTags",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "PostTags",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedTime",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRemoved",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Posts",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModifiedTime",
                table: "PostComments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateRemoved",
                table: "PostComments",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "PostComments",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Tags_TagId",
                table: "PostTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Value",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
