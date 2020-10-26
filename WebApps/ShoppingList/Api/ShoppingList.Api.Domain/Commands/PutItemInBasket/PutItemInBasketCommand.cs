using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Commands.PutItemInBasket
{
    public class PutItemInBasketCommand : ICommand<bool>
    {
        public PutItemInBasketCommand(ShoppingListId shoppingListId, ShoppingListItemId itemId)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
    }
}