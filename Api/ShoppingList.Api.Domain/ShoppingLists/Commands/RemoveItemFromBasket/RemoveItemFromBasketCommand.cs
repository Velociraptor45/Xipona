using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket
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