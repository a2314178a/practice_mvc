using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class addTeaIDMaxPeoSelStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPeople",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeacherID",
                table: "Subjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<byte>(
                name: "Sex",
                table: "Students",
                type: "tinyint(1) unsigned",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Stu_subs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Stu_subs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Stu_subs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPeople",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Stu_subs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Stu_subs");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Stu_subs");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "Students",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint(1) unsigned",
                oldDefaultValue: (byte)0);
        }
    }
}
