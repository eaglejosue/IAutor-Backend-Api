using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAutor.Api.Migrations
{
    /// <inheritdoc />
    public partial class AlterQuesdtionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "questions",
                newName: "subject");

            migrationBuilder.AlterColumn<string>(
                name: "subject",
                table: "questions",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subject",
                table: "questions",
                newName: "Subject");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "questions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }
    }
}
