using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    /// <summary>
    /// Represents the normalized amount of an item for one serving.
    /// </summary>
    public class ItemAmountForOneServingContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="itemName"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantityLabel"></param>
        /// <param name="quantity"></param>
        /// <param name="defaultStoreId"></param>
        /// <param name="addToShoppingListByDefault"></param>
        /// <param name="availabilities"></param>
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

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// The ID of the item's type. Null if the item has no types.
        /// </summary>
        public Guid? ItemTypeId { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string ItemName { get; }

        /// <summary>
        /// The type of the item's quantity.
        /// </summary>
        public int QuantityType { get; }

        /// <summary>
        /// The label of the item's quantity.
        /// </summary>
        public string QuantityLabel { get; }

        /// <summary>
        /// The quantity of the item for one serving.
        /// </summary>
        public float Quantity { get; }

        /// <summary>
        /// The ID of the default store to by the item in.
        /// </summary>
        public Guid DefaultStoreId { get; }

        /// <summary>
        /// Whether the item should be added to the shopping list by default.
        /// </summary>
        public bool AddToShoppingListByDefault { get; }

        /// <summary>
        /// The availabilities of the item in different stores.
        /// </summary>
        public IEnumerable<ItemAmountForOneServingAvailabilityContract> Availabilities { get; }
    }
}