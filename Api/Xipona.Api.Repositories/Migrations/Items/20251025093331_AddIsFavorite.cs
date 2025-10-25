using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xipona.Api.Repositories.Migrations.Items
{
    /// <inheritdoc />
    public partial class AddIsFavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Items",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Items");
        }
    }
}
