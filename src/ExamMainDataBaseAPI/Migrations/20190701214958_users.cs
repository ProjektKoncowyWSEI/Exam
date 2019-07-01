using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamMainDataBaseAPI.Migrations
{
    public partial class users : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "answer",
                table: "Answer",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Answer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Answer",
                newName: "answer");
        }
    }
}
