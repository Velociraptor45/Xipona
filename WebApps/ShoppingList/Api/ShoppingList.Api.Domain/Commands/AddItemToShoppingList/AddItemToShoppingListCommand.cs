using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommand : ICommand<bool>
    {
        public AddItemToShoppingListCommand(ShoppingListId shoppingListId, ShoppingListItemId shoppingListItemId,
            float quantity)
        {
            ShoppingListId = shoppingListId;
            ShoppingListItemId = shoppingListItemId;
            Quantity = quantity;
        }

        public ShoppingListId ShoppingListId { get; }
        public ShoppingListItemId ShoppingListItemId { get; }
        public float Quantity { get; }
    }
}