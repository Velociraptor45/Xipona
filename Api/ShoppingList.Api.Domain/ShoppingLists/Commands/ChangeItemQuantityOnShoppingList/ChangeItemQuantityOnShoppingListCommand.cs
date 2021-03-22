using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommand : ICommand<bool>
    {
        public ChangeItemQuantityOnShoppingListCommand(ShoppingListId shoppingListId, ItemId itemId,
            float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ItemId ItemId { get; }
        public float Quantity { get; }
    }
}