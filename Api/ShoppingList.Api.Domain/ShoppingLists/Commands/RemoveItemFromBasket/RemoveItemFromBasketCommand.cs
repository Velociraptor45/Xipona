using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommand : ICommand<bool>
    {
        public RemoveItemFromBasketCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
            ItemTypeId? itemTypeId)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            OfflineTolerantItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            ItemTypeId = itemTypeId;
        }

        public ShoppingListId ShoppingListId { get; }
        public OfflineTolerantItemId OfflineTolerantItemId { get; }
        public ItemTypeId? ItemTypeId { get; }
    }
}