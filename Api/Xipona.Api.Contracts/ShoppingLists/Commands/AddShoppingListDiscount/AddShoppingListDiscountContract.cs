namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddShoppingListDiscount
{
    /// <summary>
    /// Represents a one-time discount for an entire shopping list.
    /// </summary>
    public class AddShoppingListDiscountContract
    {
        /// <summary>
        /// Creates an object where either the <paramref name="discountPrice"/> XOR the <paramref name="discountPercentage"/>
        /// can be filled. It's not allowed to fill none of them or both.
        /// </summary>
        /// <param name="discountPrice"></param>
        /// <param name="discountPercentage"></param>
        public AddShoppingListDiscountContract(decimal? discountPrice, decimal? discountPercentage)
        {

        }

        /// <summary>
        /// The price by which the shopping list is discounted
        /// </summary>
        public decimal? DiscountPrice { get; }


        /// <summary>
        /// The percentage by which the shopping list is discounted
        /// </summary>
        public decimal? DiscountPercentage { get; }
    }
}
