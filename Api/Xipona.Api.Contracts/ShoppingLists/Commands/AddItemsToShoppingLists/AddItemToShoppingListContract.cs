using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists
{
    /// <summary>
    /// Represents a contract to add an item to a shopping list.
    /// </summary>
    public class AddItemToShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="storeId"></param>
        /// <param name="quantity"></param>
        public AddItemToShoppingListContract(Guid itemId, Guid? itemTypeId, Guid storeId, float quantity)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            StoreId = storeId;
            Quantity = quantity;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The ID of the item type.
        /// Null if the item does not have types.
        /// </summary>
        public Guid? ItemTypeId { get; set; }

        /// <summary>
        /// The ID of the store to whose shopping list the item should be added.
        /// </summary>
        public Guid StoreId { get; set; }

        /// <summary>
        /// The quantity of the item to add.
        /// </summary>
        public float Quantity { get; set; }
    }
}