using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIExtensionsCenter.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "Extensions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "Extensions");
        }
    }
}
