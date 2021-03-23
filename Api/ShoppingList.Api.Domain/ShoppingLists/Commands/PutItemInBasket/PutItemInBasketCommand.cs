using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketCommand : ICommand<bool>
    {
        public PutItemInBasketCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        }

        public ShoppingListId ShoppingListId { get; }
        public OfflineTolerantItemId ItemId { get; }
    }
}