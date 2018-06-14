using Microsoft.EntityFrameworkCore.Migrations;

namespace Dozentenplanung.Migrations
{
    public partial class dummyExamType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDummy",
                table: "ExamTypes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDummy",
                table: "ExamTypes");
        }
    }
}
