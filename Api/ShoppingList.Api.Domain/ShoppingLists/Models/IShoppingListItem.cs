using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListItem
    {
        ItemId Id { get; }
        bool IsInBasket { get; }
        float Quantity { get; }
        ItemTypeId? TypeId { get; }

        IShoppingListItem PutInBasket();

        IShoppingListItem RemoveFromBasket();

        IShoppingListItem ChangeQuantity(float quantity);
    }
}