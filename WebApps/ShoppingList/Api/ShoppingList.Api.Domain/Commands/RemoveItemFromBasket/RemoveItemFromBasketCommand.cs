using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommand : ICommand<bool>
    {
        public RemoveItemFromBasketCommand(ShoppingListId shoppingListId, ShoppingListItemId itemId)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
    }
}