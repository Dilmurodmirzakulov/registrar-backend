using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WIUT.Registrar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetExistingDocumentsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Set all existing documents to active with public visibility
            migrationBuilder.Sql(@"
                UPDATE [Registrar].[DbDocuments] 
                SET [IsActive] = 1, [Visibility] = 0 
                WHERE [IsActive] = 0
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No rollback needed
        }
    }
}
