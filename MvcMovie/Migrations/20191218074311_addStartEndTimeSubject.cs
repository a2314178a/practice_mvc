using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class addStartEndTimeSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubjectTime",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "Subjects",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Subjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Subjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "WeekDay",
                table: "Subjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_SubjectName_WeekDay_StartTime_EndTime",
                table: "Subjects",
                columns: new[] { "SubjectName", "WeekDay", "StartTime", "EndTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subjects_SubjectName_WeekDay_StartTime_EndTime",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "Subjects");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectName",
                table: "Subjects",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "SubjectTime",
                table: "Subjects",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
