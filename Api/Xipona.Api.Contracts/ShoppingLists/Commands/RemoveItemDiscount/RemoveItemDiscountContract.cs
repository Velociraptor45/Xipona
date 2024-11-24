using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemDiscount
{
    /// <summary>
    /// Represents a request to remove a discount from an item on a shopping list.
    /// </summary>
    public class RemoveItemDiscountContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        public RemoveItemDiscountContract(Guid itemId, Guid? itemTypeId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        /// <summary>
        /// The ID of the item to remove the discount from.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// The ID of the item type to remove the discount from. <c>null</c> if the item does not have a type.
        /// </summary>
        public Guid? ItemTypeId { get; }
    }
}
