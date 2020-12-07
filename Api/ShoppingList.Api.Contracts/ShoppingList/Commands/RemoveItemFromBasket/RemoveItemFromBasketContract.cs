using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketContract
    {
        public int ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
    }
}