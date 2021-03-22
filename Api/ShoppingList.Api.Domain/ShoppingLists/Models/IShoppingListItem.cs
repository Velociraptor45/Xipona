namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingListItem
    {
        ShoppingListItemId Id { get; }
        bool IsInBasket { get; }
        float Quantity { get; }

        IShoppingListItem PutInBasket();

        IShoppingListItem RemoveFromBasket();

        IShoppingListItem ChangeQuantity(float quantity);
    }
}