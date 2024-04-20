#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Recipes
{
    public partial class AddDefaultItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultItemId",
                table: "Ingredients",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultItemTypeId",
                table: "Ingredients",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultItemId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "DefaultItemTypeId",
                table: "Ingredients");
        }
    }
}
