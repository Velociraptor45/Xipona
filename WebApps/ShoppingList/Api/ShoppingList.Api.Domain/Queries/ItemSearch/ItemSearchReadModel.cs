using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Queries.ItemSearch
{
    public class ItemSearchReadModel
    {
        public ItemSearchReadModel(StoreItemActualId id, string name, float price, Manufacturer manufacturer,
            ItemCategory itemCategory)
        {
            Id = id;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
            ItemCategory = itemCategory;
        }

        public StoreItemActualId Id { get; }
        public string Name { get; }
        public float Price { get; }
        public Manufacturer Manufacturer { get; }
        public ItemCategory ItemCategory { get; }
    }
}