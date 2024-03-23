#nullable disable

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Items;

public partial class ChangeAvailableAtPKs : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemTypes_ItemTypeId",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropIndex(
            name: "IX_ItemTypeAvailableAts_ItemTypeId",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AvailableAts",
            table: "AvailableAts");

        migrationBuilder.DropForeignKey(
            name: "FK_AvailableAts_Items_ItemId",
            table: "AvailableAts");

        migrationBuilder.DropIndex(
            name: "IX_AvailableAts_ItemId",
            table: "AvailableAts");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropColumn(
            name: "Id",
            table: "AvailableAts");

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .Annotation("Relational:ColumnOrder", 2);

        migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .Annotation("Relational:ColumnOrder", 1);

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .Annotation("Relational:ColumnOrder", 2);

        migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .Annotation("Relational:ColumnOrder", 1);

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts",
            columns: new[] { "ItemTypeId", "StoreId" });

        migrationBuilder.AddPrimaryKey(
            name: "PK_AvailableAts",
            table: "AvailableAts",
            columns: new[] { "ItemId", "StoreId" });

        migrationBuilder.AddForeignKey(
            name: "FK_ItemTypeAvailableAts_ItemTypes_ItemTypeId",
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AvailableAts",
            table: "AvailableAts");

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("Relational:ColumnOrder", 2);

        migrationBuilder.AlterColumn<int>(
                name: "ItemTypeId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("Relational:ColumnOrder", 1);

        migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                defaultValue: 0)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("Relational:ColumnOrder", 2);

        migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
            .OldAnnotation("Relational:ColumnOrder", 1);

        migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                defaultValue: 0)
            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

        migrationBuilder.AddPrimaryKey(
            name: "PK_ItemTypeAvailableAts",
            table: "ItemTypeAvailableAts",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AvailableAts",
            table: "AvailableAts",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_ItemTypeAvailableAts_ItemTypeId",
            table: "ItemTypeAvailableAts",
            column: "ItemTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_AvailableAts_ItemId",
            table: "AvailableAts",
            column: "ItemId");
    }
}