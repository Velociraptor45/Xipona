using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.StoreItems;

public partial class ChangeIdsToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypes_ItemTypes_PredecessorId",
            table: "ItemTypes");

        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypes_Items_ItemId",
            table: "ItemTypes");

        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemTypes_ItemTypeId",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropForeignKey(
            name: "FK_AvailableAts_Items_ItemId",
            table: "AvailableAts");

        migrationBuilder.DropForeignKey(
            name: "FK_Items_Items_PredecessorId",
            table: "Items");

        migrationBuilder.AlterColumn<Guid>(
            name: "PredecessorId",
            table: "ItemTypes",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemId",
            table: "ItemTypes",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ItemTypes",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemTypeId",
            table: "ItemTypeAvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "PredecessorId",
            table: "Items",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Items",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AlterColumn<Guid>(
            name: "ItemId",
            table: "AvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypes_ItemTypes_PredecessorId",
            table: "ItemTypes",
            column: "PredecessorId",
            principalTable: "ItemTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypes_Items_ItemId",
            table: "ItemTypes",
            column: "ItemId",
            principalTable: "Items",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemType_ItemTypeId",
            table: "ItemTypeAvailableAts",
            column: "ItemTypeId",
            principalTable: "ItemTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_AvailableAts_Items_ItemId",
            table: "AvailableAts",
            column: "ItemId",
            principalTable: "Items",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_Items_Items_PredecessorId",
            table: "Items",
            column: "PredecessorId",
            principalTable: "Items",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
                name: "PredecessorId",
                table: "ItemTypes",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "ItemTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ItemTypes",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "PredecessorId",
                table: "Items",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");
    }
}