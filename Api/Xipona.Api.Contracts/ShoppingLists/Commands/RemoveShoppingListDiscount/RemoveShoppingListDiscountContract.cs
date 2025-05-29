using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveShoppingListDiscount
{
    /// <summary>
    /// Represents a request to remove a discount from a shopping list.
    /// </summary>
    public class RemoveShoppingListDiscountContract
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="discountId"></param>
        public RemoveShoppingListDiscountContract(Guid discountId)
        {
            DiscountId = discountId;
        }

        /// <summary>
        /// The ID of the discount to remove from the shopping list.
        /// </summary>
        public Guid DiscountId { get; }
    }
}
