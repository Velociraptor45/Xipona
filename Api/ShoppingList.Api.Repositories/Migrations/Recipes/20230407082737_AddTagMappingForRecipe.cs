using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.Recipes
{
    /// <inheritdoc />
    public partial class AddTagMappingForRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagsForRecipes",
                columns: table => new
                {
                    RecipeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RecipeTagId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagsForRecipes", x => new { x.RecipeId, x.RecipeTagId });
                    table.ForeignKey(
                        name: "FK_TagsForRecipes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagsForRecipes");
        }
    }
}
