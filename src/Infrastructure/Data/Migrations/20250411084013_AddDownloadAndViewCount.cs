using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIExtensionsCenter.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDownloadAndViewCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Extensions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Extensions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Extensions");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Extensions");
        }
    }
}
