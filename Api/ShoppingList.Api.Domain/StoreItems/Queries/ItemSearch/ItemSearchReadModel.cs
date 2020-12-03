using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch
{
    public class ItemSearchReadModel
    {
        public ItemSearchReadModel(StoreItemActualId id, string name, int defaultQuantity, float price, Manufacturer manufacturer,
            ItemCategory itemCategory)
        {
            Id = id;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            Manufacturer = manufacturer;
            ItemCategory = itemCategory;
        }

        public StoreItemActualId Id { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public Manufacturer Manufacturer { get; }
        public ItemCategory ItemCategory { get; }
    }
}