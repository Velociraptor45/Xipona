using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.StoreItems;

public partial class ChangeCategoryAndManufacturerIdsToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "ManufacturerId",
            table: "Items",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemCategoryId",
            table: "Items",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
                name: "ManufacturerId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemCategoryId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");
    }
}