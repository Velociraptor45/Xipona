namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListItem : IShoppingListItem
    {
        public ShoppingListItem(ShoppingListItemId id, bool isInBasket, float quantity)
        {
            Id = id;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public ShoppingListItemId Id { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }

        public IShoppingListItem PutInBasket()
        {
            return new ShoppingListItem(Id, true, Quantity);
        }

        public IShoppingListItem RemoveFromBasket()
        {
            return new ShoppingListItem(Id, false, Quantity);
        }

        public IShoppingListItem ChangeQuantity(float quantity)
        {
            return new ShoppingListItem(Id, IsInBasket, quantity);
        }
    }
}