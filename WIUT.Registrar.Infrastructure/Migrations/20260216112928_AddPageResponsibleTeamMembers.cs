using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPageResponsibleTeamMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageTeamMembers",
                schema: "Registrar",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false),
                    TeamMemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageTeamMembers", x => new { x.PageId, x.TeamMemberId });
                    table.ForeignKey(
                        name: "FK_PageTeamMembers_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "Registrar",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageTeamMembers_TeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalSchema: "Registrar",
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageTeamMembers_TeamMemberId",
                schema: "Registrar",
                table: "PageTeamMembers",
                column: "TeamMemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageTeamMembers",
                schema: "Registrar");
        }
    }
}
