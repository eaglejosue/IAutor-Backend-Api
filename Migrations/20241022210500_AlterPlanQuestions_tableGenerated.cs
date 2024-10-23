using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAutor.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterPlanQuestions_tableGenerated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanQuestion_plans_PlanId",
                table: "PlanQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_PlanQuestion_questions_QuestionId",
                table: "PlanQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanQuestion",
                table: "PlanQuestion");

            migrationBuilder.RenameTable(
                name: "PlanQuestion",
                newName: "plan_question");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "plan_question",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "plan_question",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "plan_question",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "plan_question",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "plan_question",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_PlanQuestion_QuestionId",
                table: "plan_question",
                newName: "IX_plan_question_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanQuestion_PlanId",
                table: "plan_question",
                newName: "IX_plan_question_PlanId");

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "plan_question",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "plan_question",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "deleted_at",
                table: "plan_question",
                type: "timestamp",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "plan_question",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_plan_question",
                table: "plan_question",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_plans_PlanId",
                table: "plan_question");

            migrationBuilder.DropForeignKey(
                name: "FK_plan_question_questions_QuestionId",
                table: "plan_question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_plan_question",
                table: "plan_question");

            migrationBuilder.RenameTable(
                name: "plan_question",
                newName: "PlanQuestion");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "PlanQuestion",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "PlanQuestion",
                newName: "UpdatedBy");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "PlanQuestion",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "PlanQuestion",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "PlanQuestion",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_QuestionId",
                table: "PlanQuestion",
                newName: "IX_PlanQuestion_QuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_plan_question_PlanId",
                table: "PlanQuestion",
                newName: "IX_PlanQuestion_PlanId");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "PlanQuestion",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "PlanQuestion",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DeletedAt",
                table: "PlanQuestion",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PlanQuestion",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanQuestion",
                table: "PlanQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanQuestion_plans_PlanId",
                table: "PlanQuestion",
                column: "PlanId",
                principalTable: "plans",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanQuestion_questions_QuestionId",
                table: "PlanQuestion",
                column: "QuestionId",
                principalTable: "questions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
