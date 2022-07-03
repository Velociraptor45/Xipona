using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsForShoppingLists
{
    public class SearchItemForShoppingListResultContract
    {
        public SearchItemForShoppingListResultContract(Guid id, Guid? typeId, string name, int defaultQuantity, float price,
            string itemCategoryName, string manufacturerName, StoreSectionContract defaultSection)
        {
            Id = id;
            TypeId = typeId;
            Name = name;
            DefaultQuantity = defaultQuantity;
            Price = price;
            ItemCategoryName = itemCategoryName;
            ManufacturerName = manufacturerName;
            DefaultSection = defaultSection;
        }

        public Guid Id { get; }
        public Guid? TypeId { get; }
        public string Name { get; }
        public int DefaultQuantity { get; }
        public float Price { get; }
        public string ItemCategoryName { get; }
        public string ManufacturerName { get; }
        public StoreSectionContract DefaultSection { get; }
    }
}