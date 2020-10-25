using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.RemoveItemFromShoppingList
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