using ShoppingList.Api.Contracts.Commands.Shared;

namespace ShoppingList.Api.Contracts.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
        public float Quantity { get; set; }
    }
}