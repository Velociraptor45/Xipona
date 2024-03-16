using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.ShoppingLists
{
    /// <inheritdoc />
    public partial class MakeCreatedAtRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 UPDATE ShoppingLists
                                 SET CreatedAt = '2000-01-01'
                                 WHERE CreatedAt IS NULL;
                                 """);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ShoppingLists",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "ShoppingLists",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");
        }
    }
}