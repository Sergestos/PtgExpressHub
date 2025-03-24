using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PtgExpressHub.Domain.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationBuilds",
                columns: table => new
                {
                    ApplicationBuildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationBuildProductionName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ApplicationBuildUserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ApplicationRepositoryUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationBuilds", x => x.ApplicationBuildId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationBuildVersions",
                columns: table => new
                {
                    ApplicationVersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationBuildId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    BlobUrl = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ChangeLog = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationBuildVersions", x => x.ApplicationVersionId);
                    table.ForeignKey(
                        name: "FK_ApplicationBuildVersions_ApplicationBuilds_ApplicationBuildId",
                        column: x => x.ApplicationBuildId,
                        principalTable: "ApplicationBuilds",
                        principalColumn: "ApplicationBuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationBuildVersions_ApplicationBuildId",
                table: "ApplicationBuildVersions",
                column: "ApplicationBuildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationBuildVersions");

            migrationBuilder.DropTable(
                name: "ApplicationBuilds");
        }
    }
}
