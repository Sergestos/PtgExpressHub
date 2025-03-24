using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PtgExpressHub.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChangeLog",
                table: "ApplicationBuildVersions",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChangeLog",
                table: "ApplicationBuildVersions",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048);
        }
    }
}
