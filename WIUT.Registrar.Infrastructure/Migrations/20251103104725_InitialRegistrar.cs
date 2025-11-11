using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialRegistrar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Registrar");

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalSchema: "Registrar",
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DbDocuments",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    ProgrammeCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KPIReports",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TagsCsv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    BodyHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                schema: "Registrar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Registrar",
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                schema: "Registrar",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_News_Slug",
                schema: "Registrar",
                table: "News",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Slug",
                schema: "Registrar",
                table: "Pages",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_DepartmentId",
                schema: "Registrar",
                table: "TeamMembers",
                column: "DepartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbDocuments",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "KPIReports",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "News",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "Pages",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "TeamMembers",
                schema: "Registrar");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Registrar");
        }
    }
}
