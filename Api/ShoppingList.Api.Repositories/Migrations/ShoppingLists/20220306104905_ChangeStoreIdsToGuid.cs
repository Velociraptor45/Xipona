#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.ShoppingLists;

public partial class ChangeStoreIdsToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "StoreId",
            table: "ShoppingLists",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "SectionId",
            table: "ItemsOnLists",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ShoppingLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "ItemsOnLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");
    }
}