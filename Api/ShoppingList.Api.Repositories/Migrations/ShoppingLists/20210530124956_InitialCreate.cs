using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.ShoppingLists;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "ShoppingLists",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                CompletionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                StoreId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ShoppingLists", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ItemsOnLists",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                ShoppingListId = table.Column<int>(type: "int", nullable: false),
                ItemId = table.Column<int>(type: "int", nullable: false),
                InBasket = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Quantity = table.Column<float>(type: "float", nullable: false),
                SectionId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ItemsOnLists", x => x.Id);
                table.ForeignKey(
                    name: "FK_ItemsOnLists_ShoppingLists_ShoppingListId",
                    column: x => x.ShoppingListId,
                    principalTable: "ShoppingLists",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ItemsOnLists_ShoppingListId",
            table: "ItemsOnLists",
            column: "ShoppingListId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ItemsOnLists");

        migrationBuilder.DropTable(
            name: "ShoppingLists");
    }
}