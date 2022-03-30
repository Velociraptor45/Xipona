using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using System;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists
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