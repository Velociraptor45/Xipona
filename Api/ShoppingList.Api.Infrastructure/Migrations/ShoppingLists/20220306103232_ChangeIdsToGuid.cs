using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.ShoppingLists;

public partial class ChangeIdsToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemsOnLists_ShoppingLists_ShoppingListId",
            table: "ItemsOnLists");

        migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ShoppingLists",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AlterColumn<Guid>(
            name: "ShoppingListId",
            table: "ItemsOnLists",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemTypeId",
            table: "ItemsOnLists",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemId",
            table: "ItemsOnLists",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemsOnLists_ShoppingLists_ShoppingListId",
            table: "ItemsOnLists",
            column: "ShoppingListId",
            principalTable: "ShoppingLists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemsOnLists_ShoppingLists_ShoppingListId",
            table: "ItemsOnLists");

        migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShoppingLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ShoppingListId",
                table: "ItemsOnLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "ItemsOnLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemsOnLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemsOnLists_ShoppingLists_ShoppingListId",
            table: "ItemsOnLists",
            column: "ShoppingListId",
            principalTable: "ShoppingLists",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}