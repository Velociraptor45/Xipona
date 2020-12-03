using ShoppingList.Api.Contracts.Commands.Shared;

namespace ShoppingList.Api.Contracts.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
    }
}