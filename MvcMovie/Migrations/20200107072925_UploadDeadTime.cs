using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class UploadDeadTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDeadLine",
                table: "SubHomeworks");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDeadTime",
                table: "SubHomeworks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadDeadTime",
                table: "SubHomeworks");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDeadLine",
                table: "SubHomeworks",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
