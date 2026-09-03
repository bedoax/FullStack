using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatedByTeacherForQuizAndQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByTeacherId",
                table: "Quizzes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedByTeacherId",
                table: "Questions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_CreatedByTeacherId",
                table: "Quizzes",
                column: "CreatedByTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CreatedByTeacherId",
                table: "Questions",
                column: "CreatedByTeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Users_CreatedByTeacherId",
                table: "Questions",
                column: "CreatedByTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_Users_CreatedByTeacherId",
                table: "Quizzes",
                column: "CreatedByTeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Users_CreatedByTeacherId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_Users_CreatedByTeacherId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_CreatedByTeacherId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CreatedByTeacherId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CreatedByTeacherId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "CreatedByTeacherId",
                table: "Questions");
        }
    }
}
