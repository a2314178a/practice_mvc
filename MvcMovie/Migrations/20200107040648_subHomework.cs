using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class subHomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworkURLs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubHomeworkID = table.Column<int>(nullable: false),
                    URL = table.Column<string>(nullable: true),
                    OriginalName = table.Column<string>(nullable: true),
                    NewName = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworkURLs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubHomeworks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubjectID = table.Column<int>(nullable: false),
                    SubjectName = table.Column<string>(nullable: true),
                    HomeworkName = table.Column<string>(nullable: true),
                    HomeworkDeatil = table.Column<string>(nullable: true),
                    UploadDeadLine = table.Column<DateTime>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubHomeworks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkURLs_NewName",
                table: "HomeworkURLs",
                column: "NewName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeworkURLs");

            migrationBuilder.DropTable(
                name: "SubHomeworks");
        }
    }
}
