using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommand : ICommand<bool>
    {
        public RemoveItemFromShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId)
        {
            ShoppingListId = shoppingListId;
            ItemId = itemId;
        }

        public ShoppingListId ShoppingListId { get; }
        public OfflineTolerantItemId ItemId { get; }
    }
}