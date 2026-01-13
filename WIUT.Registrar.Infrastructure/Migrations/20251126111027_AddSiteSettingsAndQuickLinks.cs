using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteSettingsAndQuickLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.CreateTable(
                    name: "QuickLinks",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Title = table.Column<string>(type: "TEXT", nullable: false),
                        Description = table.Column<string>(type: "TEXT", nullable: true),
                        LinkUrl = table.Column<string>(type: "TEXT", nullable: false),
                        IconKey = table.Column<string>(type: "TEXT", nullable: false),
                        ThemeKey = table.Column<string>(type: "TEXT", nullable: false),
                        DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false),
                        IsExternal = table.Column<bool>(type: "INTEGER", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                        IsPublished = table.Column<bool>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuickLinks", x => x.Id);
                    });

                migrationBuilder.CreateTable(
                    name: "SiteSettings",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "INTEGER", nullable: false)
                            .Annotation("Sqlite:Autoincrement", true),
                        Key = table.Column<string>(type: "TEXT", nullable: false),
                        Value = table.Column<string>(type: "TEXT", nullable: true),
                        Category = table.Column<string>(type: "TEXT", nullable: true),
                        Description = table.Column<string>(type: "TEXT", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                        IsPublished = table.Column<bool>(type: "INTEGER", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_SiteSettings", x => x.Id);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_QuickLinks_DisplayOrder",
                    table: "QuickLinks",
                    column: "DisplayOrder");

                migrationBuilder.CreateIndex(
                    name: "IX_SiteSettings_Key",
                    table: "SiteSettings",
                    column: "Key",
                    unique: true);
            }
            else
            {
                migrationBuilder.CreateTable(
                    name: "QuickLinks",
                    schema: "Registrar",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        IconKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ThemeKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        DisplayOrder = table.Column<int>(type: "int", nullable: false),
                        IsExternal = table.Column<bool>(type: "bit", nullable: false),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                        IsPublished = table.Column<bool>(type: "bit", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_QuickLinks", x => x.Id);
                    });

                migrationBuilder.CreateTable(
                    name: "SiteSettings",
                    schema: "Registrar",
                    columns: table => new
                    {
                        Id = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                        Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                        UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                        IsPublished = table.Column<bool>(type: "bit", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_SiteSettings", x => x.Id);
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_QuickLinks_DisplayOrder",
                    schema: "Registrar",
                    table: "QuickLinks",
                    column: "DisplayOrder");

                migrationBuilder.CreateIndex(
                    name: "IX_SiteSettings_Key",
                    schema: "Registrar",
                    table: "SiteSettings",
                    column: "Key",
                    unique: true);
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder.ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.DropTable(
                    name: "QuickLinks");

                migrationBuilder.DropTable(
                    name: "SiteSettings");
            }
            else
            {
                migrationBuilder.DropTable(
                    name: "QuickLinks",
                    schema: "Registrar");

                migrationBuilder.DropTable(
                    name: "SiteSettings",
                    schema: "Registrar");
            }
        }
    }
}
