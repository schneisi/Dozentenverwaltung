using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dozentenplanung.Migrations
{
    public partial class unitDateInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quarter",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Units");

            migrationBuilder.AddColumn<DateTime>(
                name: "BeginDate",
                table: "Units",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Units",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BeginDate",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "Quarter",
                table: "Units",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Units",
                nullable: false,
                defaultValue: 0);
        }
    }
}
