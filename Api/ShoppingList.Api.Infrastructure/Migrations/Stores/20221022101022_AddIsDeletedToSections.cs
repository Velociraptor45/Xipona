using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.Stores
{
    public partial class AddIsDeletedToSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sections",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sections");
        }
    }
}
