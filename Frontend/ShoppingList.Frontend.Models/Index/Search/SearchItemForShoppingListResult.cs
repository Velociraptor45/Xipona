namespace ProjectHermes.ShoppingList.Frontend.Models.Index.Search
{
    public class SearchItemForShoppingListResult
    {
        public SearchItemForShoppingListResult(int itemId, int? itemTypeId, string name, float price, string priceLabel,
            string itemCategoryName, string manufacturerName, int defaultSectionId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Name = name;
            Price = price;
            PriceLabel = priceLabel;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSectionId = defaultSectionId;
        }

        public int ItemId { get; set; }
        public int? ItemTypeId { get; }
        public string Name { get; }
        public float Price { get; }
        public string PriceLabel { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
        public int DefaultSectionId { get; }

        public string DisplayValue => $"{Name} | {ManufacturerName} | {Price}{PriceLabel}";
    }
}