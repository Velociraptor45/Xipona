using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Recipes
{
    /// <inheritdoc />
    public partial class AddRecipeSideDish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SideDishId",
                table: "Recipes",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_SideDishId",
                table: "Recipes",
                column: "SideDishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_Recipes_SideDishId",
                table: "Recipes",
                column: "SideDishId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_Recipes_SideDishId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_SideDishId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "SideDishId",
                table: "Recipes");
        }
    }
}
