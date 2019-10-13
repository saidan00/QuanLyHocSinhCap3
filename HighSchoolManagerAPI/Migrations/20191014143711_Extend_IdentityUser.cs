using Microsoft.EntityFrameworkCore.Migrations;

namespace HighSchoolManagerAPI.Migrations
{
    public partial class Extend_IdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherID",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherID",
                table: "AspNetUsers",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Teachers_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TeacherID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "AspNetUsers");
        }
    }
}
