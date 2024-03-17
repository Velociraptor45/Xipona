#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.ShoppingLists;

public partial class AddItemTypeId : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletionDate",
            table: "ShoppingLists",
            type: "datetime(6)",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "SectionId",
            table: "ItemsOnLists",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AddColumn<int>(
            name: "ItemTypeId",
            table: "ItemsOnLists",
            type: "int",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ItemTypeId",
            table: "ItemsOnLists");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletionDate",
            table: "ShoppingLists",
            type: "datetime",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime(6)",
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "SectionId",
            table: "ItemsOnLists",
            type: "int",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");
    }
}