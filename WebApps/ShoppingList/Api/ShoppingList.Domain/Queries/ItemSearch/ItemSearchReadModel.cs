using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Queries.ItemSearch
{
    public class ItemSearchReadModel
    {
        public ItemSearchReadModel(StoreItemId id, string name, float price, Manufacturer manufacturer,
            ItemCategory itemCategory)
        {
            Id = id;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
            ItemCategory = itemCategory;
        }

        public StoreItemId Id { get; }
        public string Name { get; }
        public float Price { get; }
        public Manufacturer Manufacturer { get; }
        public ItemCategory ItemCategory { get; }
    }
}