using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExamMainDataBaseAPI.Migrations
{
    public partial class exams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Questions_QuestionsId",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Answer",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Questions",
                table: "QuestionAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerType",
                table: "AnswersType");

            migrationBuilder.DropIndex(
                name: "IX_Answer_QuestionsId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "Question",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionsId",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "QuestionID",
                table: "QuestionAnswer",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "AnswerID",
                table: "QuestionAnswer",
                newName: "AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_QuestionID",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_QuestionId");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "Answer",
                newName: "answer");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Answer",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "Questions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Questions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "AnswersType",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "answer",
                table: "Answer",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswersType",
                table: "AnswersType",
                column: "AnswerType");

            migrationBuilder.CreateTable(
                name: "Exams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    MinStart = table.Column<DateTime>(nullable: false),
                    MaxStart = table.Column<DateTime>(nullable: false),
                    DurationMinutes = table.Column<int>(nullable: false),
                    MaxPoints = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Login = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Login);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Answer_AnswerId",
                table: "QuestionAnswer",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Questions_QuestionId",
                table: "QuestionAnswer",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Answer_AnswerId",
                table: "QuestionAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswer_Questions_QuestionId",
                table: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "Exams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswersType",
                table: "AnswersType");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "QuestionAnswer",
                newName: "QuestionID");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                table: "QuestionAnswer",
                newName: "AnswerID");

            migrationBuilder.RenameIndex(
                name: "IX_QuestionAnswer_QuestionId",
                table: "QuestionAnswer",
                newName: "IX_QuestionAnswer_QuestionID");

            migrationBuilder.RenameColumn(
                name: "answer",
                table: "Answer",
                newName: "Answer");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Answer",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "Questions",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Questions",
                unicode: false,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AnswerType",
                table: "AnswersType",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Answer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionsId",
                table: "Answer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerType",
                table: "AnswersType",
                column: "AnswerType");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionsId",
                table: "Answer",
                column: "QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Questions_QuestionsId",
                table: "Answer",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Answer",
                table: "QuestionAnswer",
                column: "AnswerID",
                principalTable: "Answer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswer_Questions",
                table: "QuestionAnswer",
                column: "QuestionID",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
