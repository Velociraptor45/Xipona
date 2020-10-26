using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommand : ICommand<bool>
    {
        public ChangeItemQuantityOnShoppingListCommand(ShoppingListId shoppingListId, ShoppingListItemId itemId,
            float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
        public float Quantity { get; }
    }
}