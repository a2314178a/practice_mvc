using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MvcMovie.Migrations
{
    public partial class addStu_subTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stu_subs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentID = table.Column<uint>(type: "int(11) unsigned", nullable: false, defaultValue: 0u),
                    SubjectID = table.Column<uint>(type: "int(11) unsigned", nullable: false, defaultValue: 0u)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stu_subs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stu_subs_StudentID_SubjectID",
                table: "Stu_subs",
                columns: new[] { "StudentID", "SubjectID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stu_subs");
        }
    }
}
