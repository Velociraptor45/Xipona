using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Xipona.Api.Repositories.Migrations.ShoppingLists
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletionDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discount_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemsOnLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    InBasket = table.Column<bool>(type: "boolean", nullable: false),
                    Quantity = table.Column<float>(type: "real", nullable: false),
                    SectionId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ShoppingListDiscount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ShoppingListId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountPercentage = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListDiscount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingListDiscount_ShoppingLists_ShoppingListId",
                        column: x => x.ShoppingListId,
                        principalTable: "ShoppingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ItemId",
                table: "Discount",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ItemTypeId",
                table: "Discount",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_ShoppingListId",
                table: "Discount",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsOnLists_ShoppingListId",
                table: "ItemsOnLists",
                column: "ShoppingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListDiscount_ShoppingListId",
                table: "ShoppingListDiscount",
                column: "ShoppingListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "ItemsOnLists");

            migrationBuilder.DropTable(
                name: "ShoppingListDiscount");

            migrationBuilder.DropTable(
                name: "ShoppingLists");
        }
    }
}
