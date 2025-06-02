using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Queries.GetActiveShoppingListByStoreId
{
    /// <summary>
    /// Represents a discount applied to a shopping list.
    /// </summary>
    public class ShoppingListDiscountContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="discountPrice"></param>
        /// <param name="discountPercentage"></param>
        public ShoppingListDiscountContract(Guid id, decimal? discountPrice, decimal? discountPercentage)
        {
            Id = id;
            DiscountPrice = discountPrice;
            DiscountPercentage = discountPercentage;
        }

        /// <summary>
        /// The ID of the shopping list discount.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// The price by which the shopping list is discounted.
        /// </summary>
        public decimal? DiscountPrice { get; }

        /// <summary>
        /// The percentage by which the shopping list is discounted.
        /// </summary>
        public decimal? DiscountPercentage { get; }
    }
}
