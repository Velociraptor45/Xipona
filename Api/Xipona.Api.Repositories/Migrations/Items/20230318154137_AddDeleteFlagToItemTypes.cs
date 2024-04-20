using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Items
{
    public partial class AddDeleteFlagToItemTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ItemTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemTypes");
        }
    }
}