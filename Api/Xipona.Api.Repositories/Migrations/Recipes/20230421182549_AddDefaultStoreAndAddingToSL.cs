using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectHermes.Xipona.Api.Repositories.Migrations.Recipes
{
    /// <inheritdoc />
    public partial class AddDefaultStoreAndAddingToSL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddToShoppingListByDefault",
                table: "Ingredients",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DefaultStoreId",
                table: "Ingredients",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.Sql(
                """
                UPDATE Ingredients ing
                INNER JOIN (
                   SELECT MIN(av.StoreId) StoreId, it.Id ItemId FROM Items it
                   INNER JOIN AvailableAts av ON av.ItemId = it.Id
                   GROUP BY it.Id
                   ) q ON q.ItemId = ing.DefaultItemId
                SET ing.DefaultStoreId = q.StoreId
                WHERE ing.DefaultItemTypeId IS NULL
                """);

            migrationBuilder.Sql(
                """
                UPDATE Ingredients ing
                INNER JOIN (
                	SELECT MIN(av.StoreId) StoreId, its.Id ItemTypeId FROM Items it
                	INNER JOIN ItemTypes its ON its.ItemId = it.Id
                	INNER JOIN ItemTypeAvailableAts av ON av.ItemTypeId = its.Id
                	GROUP BY its.Id
                	) q ON q.ItemTypeId = ing.DefaultItemTypeId
                SET ing.DefaultStoreId = q.StoreId
                WHERE ing.DefaultItemTypeId IS NOT NULL
                """);

            migrationBuilder.Sql(
                """
                UPDATE Ingredients ing
                SET ing.AddToShoppingListByDefault = true
                WHERE ing.DefaultItemId IS NOT NULL
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddToShoppingListByDefault",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "DefaultStoreId",
                table: "Ingredients");
        }
    }
}