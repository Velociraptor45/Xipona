using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommand : ICommand<bool>
    {
        public ChangeItemQuantityOnShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
            ItemTypeId? itemTypeId, float quantity)
        {
            ShoppingListId = shoppingListId ?? throw new ArgumentNullException(nameof(shoppingListId));
            OfflineTolerantItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
            ItemTypeId = itemTypeId;
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public OfflineTolerantItemId OfflineTolerantItemId { get; }
        public ItemTypeId? ItemTypeId { get; }
        public float Quantity { get; }
    }
}