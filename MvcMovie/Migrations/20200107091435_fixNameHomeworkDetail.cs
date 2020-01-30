using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class fixNameHomeworkDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeworkDeatil",
                table: "SubHomeworks");

            migrationBuilder.AddColumn<string>(
                name: "HomeworkDetail",
                table: "SubHomeworks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HomeworkDetail",
                table: "SubHomeworks");

            migrationBuilder.AddColumn<string>(
                name: "HomeworkDeatil",
                table: "SubHomeworks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
