using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    /// <summary>
    /// Represents a request to change the quantity of an item on a shopping list.
    /// </summary>
    public class ChangeItemQuantityOnShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="quantity"></param>
        public ChangeItemQuantityOnShoppingListContract(ItemIdContract itemId, Guid? itemTypeId, float quantity)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public ItemIdContract ItemId { get; set; }

        /// <summary>
        /// The ID of the item type.
        /// Null if the item does not have types.
        /// </summary>
        public Guid? ItemTypeId { get; set; }

        /// <summary>
        /// The quantity of the item.
        /// </summary>
        public float Quantity { get; set; }
    }
}