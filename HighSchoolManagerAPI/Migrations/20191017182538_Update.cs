using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HighSchoolManagerAPI.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EnrollDate",
                table: "Students",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "EnrollDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Students");
        }
    }
}
