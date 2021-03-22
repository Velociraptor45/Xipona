namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListItemFactory : IShoppingListItemFactory
    {
        public IShoppingListItem Create(ShoppingListItemId id, bool isInBasket, float quantity)
        {
            return new ShoppingListItem(
                id,
                isInBasket,
                quantity);
        }
    }
}