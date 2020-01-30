using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class subIDchangeAccID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectID",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "Students",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "SubjectID",
                table: "Students",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
