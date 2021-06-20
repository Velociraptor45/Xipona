using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.ItemSearch
{
    public class ItemSearchContract
    {
        public ItemSearchContract(int id, string name, int defaultQuantity, float price,
            string itemCategoryName, string manufacturerName, StoreSectionContract defaultSection)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSection = defaultSection;
        }

        public int Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
        public StoreSectionContract DefaultSection { get; }
    }
}