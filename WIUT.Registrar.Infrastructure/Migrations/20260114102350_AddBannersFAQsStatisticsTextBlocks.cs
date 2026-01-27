using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBannersFAQsStatisticsTextBlocks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQs",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardTitle = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TextBlocks",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageType = table.Column<int>(type: "int", nullable: true),
                    SectionKey = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CssClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextBlocks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_DisplayOrder",
                schema: "Registrar",
                table: "Banners",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_SectionKey",
                schema: "Registrar",
                table: "Banners",
                column: "SectionKey");

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_DisplayOrder",
                schema: "Registrar",
                table: "FAQs",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_CardTitle",
                schema: "Registrar",
                table: "Statistics",
                column: "CardTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_DisplayOrder",
                schema: "Registrar",
                table: "Statistics",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TextBlocks_DisplayOrder",
                schema: "Registrar",
                table: "TextBlocks",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_TextBlocks_PageType",
                schema: "Registrar",
                table: "TextBlocks",
                column: "PageType");

            migrationBuilder.CreateIndex(
                name: "IX_TextBlocks_SectionKey",
                schema: "Registrar",
                table: "TextBlocks",
                column: "SectionKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "FAQs",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "Statistics",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "TextBlocks",
                schema: "Registrar");
        }
    }
}
