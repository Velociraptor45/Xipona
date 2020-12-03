using ShoppingList.Api.Contracts.Commands.Shared;

namespace ShoppingList.Api.Contracts.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
    }
}