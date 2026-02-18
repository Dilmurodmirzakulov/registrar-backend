using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelForProtectedVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DetailsHtml",
                schema: "Registrar",
                table: "TeamMembers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                schema: "Registrar",
                table: "TeamMembers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ManagerId",
                schema: "Registrar",
                table: "TeamMembers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMembers_TeamMembers_ManagerId",
                schema: "Registrar",
                table: "TeamMembers",
                column: "ManagerId",
                principalSchema: "Registrar",
                principalTable: "TeamMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMembers_TeamMembers_ManagerId",
                schema: "Registrar",
                table: "TeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_TeamMembers_ManagerId",
                schema: "Registrar",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "DetailsHtml",
                schema: "Registrar",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "Registrar",
                table: "TeamMembers");
        }
    }
}
