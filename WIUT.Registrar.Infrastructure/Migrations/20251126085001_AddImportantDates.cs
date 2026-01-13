using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImportantDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.CreateTable(
                    name: "ImportantDates",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Title = table.Column<string>(type: "TEXT", nullable: false),
                        Description = table.Column<string>(type: "TEXT", nullable: true),
                        Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                        SectionKey = table.Column<string>(type: "TEXT", nullable: false),
                        LinkUrl = table.Column<string>(type: "TEXT", nullable: true),
                        SortOrder = table.Column<int>(type: "INTEGER", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                        IsPublished = table.Column<bool>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ImportantDates", x => x.Id);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_ImportantDates_SectionKey_Date",
                    table: "ImportantDates",
                    columns: new[] { "SectionKey", "Date" });
            }
            else
            {
                migrationBuilder.CreateTable(
                    name: "ImportantDates",
                    schema: "Registrar",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                        SectionKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        SortOrder = table.Column<int>(type: "int", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                        IsPublished = table.Column<bool>(type: "bit", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ImportantDates", x => x.Id);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_ImportantDates_SectionKey_Date",
                    schema: "Registrar",
                    table: "ImportantDates",
                    columns: new[] { "SectionKey", "Date" });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.DropTable(
                    name: "ImportantDates");
            }
            else
            {
                migrationBuilder.DropTable(
                    name: "ImportantDates",
                    schema: "Registrar");
            }
        }
    }
}
