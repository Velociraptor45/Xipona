using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    public class ItemAmountForOneServingContract
    {
        public ItemAmountForOneServingContract(
            Guid itemId,
            Guid? itemTypeId,
            int quantityType,
            string quantityLabel,
            float quantity,
            Guid defaultStoreId,
            bool addToShoppingListByDefault,
            IEnumerable<ItemAmountForOneServingAvailabilityContract> availabilities)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            QuantityType = quantityType;
            QuantityLabel = quantityLabel;
            Quantity = quantity;
            DefaultStoreId = defaultStoreId;
            AddToShoppingListByDefault = addToShoppingListByDefault;
            Availabilities = availabilities;
        }

        public Guid ItemId { get; }
        public Guid? ItemTypeId { get; }
        public int QuantityType { get; }
        public string QuantityLabel { get; }
        public float Quantity { get; }
        public Guid DefaultStoreId { get; }
        public bool AddToShoppingListByDefault { get; }
        public IEnumerable<ItemAmountForOneServingAvailabilityContract> Availabilities { get; }
    }
}