using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    /// <summary>
    /// Represents a contract to remove an item from a shopping list.
    /// </summary>
    public class RemoveItemFromShoppingListContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        public RemoveItemFromShoppingListContract(ItemIdContract itemId, Guid? itemTypeId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
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
    }
}