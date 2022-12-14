using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsForShoppingLists
{
    public class SearchItemForShoppingListResultContract
    {
        public SearchItemForShoppingListResultContract(Guid id, Guid? typeId, string name, int defaultQuantity,
            float price, string priceLabel, string itemCategoryName, string manufacturerName,
            SectionContract defaultSection)
        {
            Id = id;
            TypeId = typeId;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            PriceLabel = priceLabel;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSection = defaultSection;
        }

        public Guid Id { get; }
        public Guid? TypeId { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public string PriceLabel { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
        public SectionContract DefaultSection { get; }
    }
}