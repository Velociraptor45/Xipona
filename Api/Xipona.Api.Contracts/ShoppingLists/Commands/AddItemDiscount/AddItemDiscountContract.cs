using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemDiscount
{
    /// <summary>
    /// Represents a one-time discount for a single item on a shopping list.
    /// </summary>
    public class AddItemDiscountContract
    {
        /// <summary>
        /// </summary>
        /// <param name="discountPrice"></param>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        public AddItemDiscountContract(decimal discountPrice, Guid itemId, Guid? itemTypeId)
        {
            DiscountPrice = discountPrice;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        /// <summary>
        /// The price of the item after the discount has been applied.
        /// </summary>
        public decimal DiscountPrice { get; }

        /// <summary>
        /// The ID of the item to apply the discount to.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// The ID of the item type to apply the discount to. <c>null</c> if the item does not have a type.
        /// </summary>
        public Guid? ItemTypeId { get; }
    }
}