using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Xipona.Api.Repositories.Migrations.Items
{
    /// <inheritdoc />
    public partial class AddShoppingListItemView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE VIEW ShoppingListItemView AS
                SELECT
	                i.""Id"" AS ""ItemId"",
	                it.""Id"" AS ""ItemTypeId"",
	                CONCAT_WS(' ', i.""Name"", it.""Name"") AS ""ItemName"",
	                i.""IsFavorite"" AS ""ItemIsFavorite"",
	                i.""QuantityType"" As ""ItemQuantityType"",
	                m.""Id"" AS ""ManufacturerId"",
	                m.""Name"" AS ""ManufacturerName"",
	                ic.""Id"" AS ""ItemCategoryId"",
	                ic.""Name"" AS ""ItemCategoryName"",
	                COALESCE(av.""Price"", itav.""Price"") AS ""Price"",
	                COALESCE(av.""DefaultSectionId"", itav.""DefaultSectionId"") As ""DefaultSectionId"",
	                COALESCE(av.""StoreId"", itav.""StoreId"") As ""StoreId"",
	                s.""Name"" AS ""SectionName"",
	                s.""SortIndex"" AS ""SectionSortingIndex"",
	                s.""IsDefaultSection"" AS ""SectionIsDefaultSection""
                FROM ""Items"" i
                LEFT JOIN ""ItemTypes"" it ON i.""Id"" = it.""ItemId"" AND it.""IsDeleted"" = false
                LEFT JOIN ""Manufacturers"" m ON i.""ManufacturerId"" = m.""Id""
                LEFT JOIN ""ItemCategories"" ic ON i.""ItemCategoryId"" = ic.""Id""
                LEFT JOIN ""AvailableAts"" av ON i.""Id"" = av.""ItemId""
                LEFT JOIN ""ItemTypeAvailableAts"" itav ON it.""Id"" = itav.""ItemTypeId""
                LEFT JOIN ""Sections"" s ON av.""DefaultSectionId"" = s.""Id"" OR itav.""DefaultSectionId"" = s.""Id""
                LEFT JOIN ""ShoppingLists"" sl ON (av.""StoreId"" = sl.""StoreId"" OR itav.""StoreId"" = sl.""StoreId"") AND sl.""CompletionDate"" is null
                LEFT JOIN ""ItemsOnLists"" iol ON sl.""Id"" = iol.""ShoppingListId"" AND i.""Id"" = iol.""ItemId"" AND (it.""Id"" = iol.""ItemTypeId"" OR COALESCE(it.""Id"", iol.""ItemTypeId"") is null)
                Where i.""Deleted"" = false AND i.""IsTemporary"" = false AND iol.""Id"" IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS ShoppingListItemView;");
        }
    }
}
