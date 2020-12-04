using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommand : ICommand<bool>
    {
        public RemoveItemFromShoppingListCommand(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId)
        {
            ShoppingListId = shoppingListId;
            ShoppingListItemId = shoppingListItemId;
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ShoppingListItemId { get; }
    }
}