using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.Items;

public partial class AddItemTypePredecessor : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "PredecessorId",
            table: "ItemTypes",
            type: "int",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ItemTypes_PredecessorId",
            table: "ItemTypes",
            column: "PredecessorId");

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypes_ItemTypes_PredecessorId",
            table: "ItemTypes",
            column: "PredecessorId",
            principalTable: "ItemTypes",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypes_ItemTypes_PredecessorId",
            table: "ItemTypes");

        migrationBuilder.DropIndex(
            name: "IX_ItemTypes_PredecessorId",
            table: "ItemTypes");

        migrationBuilder.DropColumn(
            name: "PredecessorId",
            table: "ItemTypes");
    }
}