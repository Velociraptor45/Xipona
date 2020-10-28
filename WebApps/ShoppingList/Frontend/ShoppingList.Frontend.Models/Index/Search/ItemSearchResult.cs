namespace ShoppingList.Frontend.Models.Index.Search
{
    public class ItemSearchResult
    {
        public ItemSearchResult(int itemId, string name, float price, string itemCategoryName, string manufacturerName)
        {
            ItemId = itemId;
            Name = name;
            Price = price;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
        }

        public int ItemId { get; }
        public string Name { get; }
        public float Price { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
    }
}