using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    public class ItemAmountForOneServingContract
    {
        public ItemAmountForOneServingContract(
            Guid itemId,
            Guid? itemTypeId,
            string itemName,
            int quantityType,
            string quantityLabel,
            float quantity,
            Guid defaultStoreId,
            bool addToShoppingListByDefault,
            IEnumerable<ItemAmountForOneServingAvailabilityContract> availabilities)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            ItemName = itemName;
            QuantityType = quantityType;
            QuantityLabel = quantityLabel;
            Quantity = quantity;
            DefaultStoreId = defaultStoreId;
            AddToShoppingListByDefault = addToShoppingListByDefault;
            Availabilities = availabilities;
        }

        public Guid ItemId { get; }
        public Guid? ItemTypeId { get; }
        public string ItemName { get; }
        public int QuantityType { get; }
        public string QuantityLabel { get; }
        public float Quantity { get; }
        public Guid DefaultStoreId { get; }
        public bool AddToShoppingListByDefault { get; }
        public IEnumerable<ItemAmountForOneServingAvailabilityContract> Availabilities { get; }
    }
}