using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPageAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.CreateTable(
                    name: "PageAttachments",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        PageId = table.Column<int>(type: "INTEGER", nullable: false),
                        Title = table.Column<string>(type: "TEXT", nullable: false),
                        Caption = table.Column<string>(type: "TEXT", nullable: true),
                        FileUrl = table.Column<string>(type: "TEXT", nullable: false),
                        FileName = table.Column<string>(type: "TEXT", nullable: false),
                        FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                        ContentType = table.Column<string>(type: "TEXT", nullable: false),
                        IsImage = table.Column<bool>(type: "INTEGER", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                        IsPublished = table.Column<bool>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PageAttachments", x => x.Id);
                        table.ForeignKey(
                            name: "FK_PageAttachments_Pages_PageId",
                            column: x => x.PageId,
                            principalTable: "Pages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_PageAttachments_PageId",
                    table: "PageAttachments",
                    column: "PageId");
            }
            else
            {
                migrationBuilder.CreateTable(
                    name: "PageAttachments",
                    schema: "Registrar",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        PageId = table.Column<int>(type: "int", nullable: false),
                        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        FileSize = table.Column<long>(type: "bigint", nullable: false),
                        ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        IsImage = table.Column<bool>(type: "bit", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                        IsPublished = table.Column<bool>(type: "bit", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PageAttachments", x => x.Id);
                        table.ForeignKey(
                            name: "FK_PageAttachments_Pages_PageId",
                            column: x => x.PageId,
                            principalSchema: "Registrar",
                            principalTable: "Pages",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_PageAttachments_PageId",
                    schema: "Registrar",
                    table: "PageAttachments",
                    column: "PageId");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.DropTable(
                    name: "PageAttachments");
            }
            else
            {
                migrationBuilder.DropTable(
                    name: "PageAttachments",
                    schema: "Registrar");
            }
        }
    }
}
