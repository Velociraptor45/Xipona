using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public interface IShoppingListItemFactory
    {
        IShoppingListItem Create(ItemId id, bool isInBasket, float quantity);
    }
}