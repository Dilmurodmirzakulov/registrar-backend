using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Registrar",
                table: "DbDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Registrar",
                table: "DbDocuments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                schema: "Registrar",
                table: "DbDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Registrar",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Registrar",
                table: "DbDocuments");

            migrationBuilder.DropColumn(
                name: "Visibility",
                schema: "Registrar",
                table: "DbDocuments");
        }
    }
}
