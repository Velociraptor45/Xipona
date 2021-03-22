namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListItemFactory
    {
        IShoppingListItem Create(ShoppingListItemId id, bool isInBasket, float quantity);
    }
}