using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.PutItemInBasket
{
    /// <summary>
    /// Represents a contract to put an item in the basket.
    /// </summary>
    public class PutItemInBasketContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        public PutItemInBasketContract(ItemIdContract itemId, Guid? itemTypeId)
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