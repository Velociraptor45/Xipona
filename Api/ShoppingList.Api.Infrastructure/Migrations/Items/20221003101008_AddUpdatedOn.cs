using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.Items
{
    public partial class AddUpdatedOn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedOn",
                table: "Items",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE Items SET UpdatedOn = '1000-01-01 00:00:00.000000' WHERE Id IN (SELECT PredecessorId FROM Items)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Items");
        }
    }
}