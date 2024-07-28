using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket
{
    /// <summary>
    /// Represents a contract to remove an item from the basket.
    /// </summary>
    public class RemoveItemFromBasketContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        public RemoveItemFromBasketContract(ItemIdContract itemId, Guid? itemTypeId)
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