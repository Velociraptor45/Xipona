#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Items;

public partial class ChangeStoreIdsToGuid : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<Guid>(
            name: "DefaultSectionId",
            table: "ItemTypeAvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "StoreId",
            table: "ItemTypeAvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "DefaultSectionId",
            table: "AvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");

        migrationBuilder.AlterColumn<Guid>(
            name: "StoreId",
            table: "AvailableAts",
            type: "char(36)",
            nullable: false,
            collation: "ascii_general_ci",
            oldClrType: typeof(int),
            oldType: "int");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
                name: "DefaultSectionId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "ItemTypeAvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "DefaultSectionId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");

        migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "AvailableAts",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)")
            .OldAnnotation("Relational:Collation", "ascii_general_ci");
    }
}