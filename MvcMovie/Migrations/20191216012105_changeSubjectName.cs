using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class changeSubjectName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "SubjectID",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectID",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Students",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
