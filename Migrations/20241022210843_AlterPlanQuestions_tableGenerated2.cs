using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAutor.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterPlanQuestions_tableGenerated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_plans_PlanId",
                table: "plan_question");

            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_questions_QuestionId",
                table: "plan_question");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "plan_question",
                newName: "question_id");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "plan_question",
                newName: "plan_id");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_QuestionId",
                table: "plan_question",
                newName: "IX_plan_question_question_id");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_PlanId",
                table: "plan_question",
                newName: "IX_plan_question_plan_id");

            migrationBuilder.AddForeignKey(
                name: "FK_plan_question_plans_plan_id",
                table: "plan_question",
                column: "plan_id",
                principalTable: "plans",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_plan_question_questions_question_id",
                table: "plan_question",
                column: "question_id",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_plans_plan_id",
                table: "plan_question");

            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_questions_question_id",
                table: "plan_question");

            migrationBuilder.RenameColumn(
                name: "question_id",
                table: "plan_question",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "plan_id",
                table: "plan_question",
                newName: "PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_question_id",
                table: "plan_question",
                newName: "IX_plan_question_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_plan_id",
                table: "plan_question",
                newName: "IX_plan_question_PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_plan_question_plans_PlanId",
                table: "plan_question",
                column: "PlanId",
                principalTable: "plans",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_plan_question_questions_QuestionId",
                table: "plan_question",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
