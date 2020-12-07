namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch
{
    public class ItemSearchContract
    {
        public ItemSearchContract(int id, string name, int defaultQuantity, float price,
            string itemCategoryName, string manufacturerName)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
    }
}