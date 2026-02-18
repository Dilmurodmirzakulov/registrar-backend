using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovePublicDocumentVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.Sql(@"
                    UPDATE [DbDocuments]
                    SET [Visibility] = 1
                    WHERE [Visibility] = 0
                ");
            }
            else
            {
                migrationBuilder.Sql(@"
                    UPDATE [Registrar].[DbDocuments]
                    SET [Visibility] = 1
                    WHERE [Visibility] = 0
                ");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (ActiveProvider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                migrationBuilder.Sql(@"
                    UPDATE [DbDocuments]
                    SET [Visibility] = 0
                    WHERE [Visibility] = 1
                ");
            }
            else
            {
                migrationBuilder.Sql(@"
                    UPDATE [Registrar].[DbDocuments]
                    SET [Visibility] = 0
                    WHERE [Visibility] = 1
                ");
            }
        }
    }
}
