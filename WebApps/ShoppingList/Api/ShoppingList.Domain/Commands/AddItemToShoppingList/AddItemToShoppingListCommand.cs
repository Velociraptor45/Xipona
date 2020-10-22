using ShoppingList.Domain.Models;

namespace ShoppingList.Domain.Commands.AddItemToShoppingList
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