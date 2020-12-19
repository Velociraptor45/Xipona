using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch
{
    public class ItemSearchReadModel
    {
        public ItemSearchReadModel(StoreItemActualId id, string name, int defaultQuantity, float price,
            ManufacturerReadModel manufacturer, ItemCategoryReadModel itemCategory)
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
        public ManufacturerReadModel Manufacturer { get; }
        public ItemCategoryReadModel ItemCategory { get; }
    }
}