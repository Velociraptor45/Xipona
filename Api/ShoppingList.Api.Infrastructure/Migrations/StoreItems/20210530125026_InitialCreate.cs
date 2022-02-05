using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.StoreItems;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Items",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                Deleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                IsTemporary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                QuantityType = table.Column<int>(type: "int", nullable: false),
                QuantityInPacket = table.Column<float>(type: "float", nullable: false),
                QuantityTypeInPacket = table.Column<int>(type: "int", nullable: false),
                ItemCategoryId = table.Column<int>(type: "int", nullable: true),
                ManufacturerId = table.Column<int>(type: "int", nullable: true),
                CreatedFrom = table.Column<Guid>(type: "char(36)", nullable: true),
                PredecessorId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Items", x => x.Id);
                table.ForeignKey(
                    name: "FK_Items_Items_PredecessorId",
                    column: x => x.PredecessorId,
                    principalTable: "Items",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "AvailableAts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                ItemId = table.Column<int>(type: "int", nullable: false),
                StoreId = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<float>(type: "float", nullable: false),
                DefaultSectionId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AvailableAts", x => x.Id);
                table.ForeignKey(
                    name: "FK_AvailableAts_Items_ItemId",
                    column: x => x.ItemId,
                    principalTable: "Items",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AvailableAts_ItemId",
            table: "AvailableAts",
            column: "ItemId");

        migrationBuilder.CreateIndex(
            name: "IX_Items_PredecessorId",
            table: "Items",
            column: "PredecessorId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AvailableAts");

        migrationBuilder.DropTable(
            name: "Items");
    }
}