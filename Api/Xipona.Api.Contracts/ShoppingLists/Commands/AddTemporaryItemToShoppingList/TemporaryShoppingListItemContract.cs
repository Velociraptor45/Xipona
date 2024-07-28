using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList
{
    /// <summary>
    /// Represents a temporary item in a shopping list.
    /// </summary>
    public class TemporaryShoppingListItemContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="isInBasket"></param>
        /// <param name="quantityInBasket"></param>
        public TemporaryShoppingListItemContract(Guid itemId, bool isInBasket, float quantityInBasket)
        {
            ItemId = itemId;
            IsInBasket = isInBasket;
            QuantityInBasket = quantityInBasket;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// Whether the item is in the basket. True if the item is in the basket, false otherwise.
        /// </summary>
        public bool IsInBasket { get; }

        /// <summary>
        /// The target quantity of the item in the basket.
        /// </summary>
        public float QuantityInBasket { get; }
    }
}