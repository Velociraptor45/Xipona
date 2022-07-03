using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.Items;

public partial class RenameItemTypeTables : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemType_Items_ItemId",
            table: "ItemType");

        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypeAvailableAt_ItemType_ItemTypeId",
            table: "ItemTypeAvailableAt");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemTypeAvailableAt",
            table: "ItemTypeAvailableAt");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemType",
            table: "ItemType");

        migrationBuilder.RenameTable(
            name: "ItemTypeAvailableAt",
            newName: "ItemTypeAvailableAts");

        migrationBuilder.RenameTable(
            name: "ItemType",
            newName: "ItemTypes");

        migrationBuilder.RenameIndex(
            name: "IX_ItemTypeAvailableAt_ItemTypeId",
            table: "ItemTypeAvailableAts",
            newName: "IX_ItemTypeAvailableAts_ItemTypeId");

        migrationBuilder.RenameIndex(
            name: "IX_ItemType_ItemId",
            table: "ItemTypes",
            newName: "IX_ItemTypes_ItemId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemTypes",
            table: "ItemTypes",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemTypes_ItemTypeId",
            table: "ItemTypeAvailableAts",
            column: "ItemTypeId",
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemTypes_ItemTypeId",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypes_Items_ItemId",
            table: "ItemTypes");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemTypes",
            table: "ItemTypes");

        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts");

        migrationBuilder.RenameTable(
            name: "ItemTypes",
            newName: "ItemType");

        migrationBuilder.RenameTable(
            name: "ItemTypeAvailableAts",
            newName: "ItemTypeAvailableAt");

        migrationBuilder.RenameIndex(
            name: "IX_ItemTypes_ItemId",
            table: "ItemType",
            newName: "IX_ItemType_ItemId");

        migrationBuilder.RenameIndex(
            name: "IX_ItemTypeAvailableAts_ItemTypeId",
            table: "ItemTypeAvailableAt",
            newName: "IX_ItemTypeAvailableAt_ItemTypeId");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemType",
            table: "ItemType",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemTypeAvailableAt",
            table: "ItemTypeAvailableAt",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemType_Items_ItemId",
            table: "ItemType",
            column: "ItemId",
            principalTable: "Items",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypeAvailableAt_ItemType_ItemTypeId",
            table: "ItemTypeAvailableAt",
            column: "ItemTypeId",
            principalTable: "ItemType",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}