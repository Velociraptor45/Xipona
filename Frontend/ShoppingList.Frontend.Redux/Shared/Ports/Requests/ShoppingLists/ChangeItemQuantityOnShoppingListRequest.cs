using ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class ChangeItemQuantityOnShoppingListRequest : IApiRequest
    {
        public ChangeItemQuantityOnShoppingListRequest(Guid requestId, Guid shoppingListId, ShoppingListItemId itemId,
            Guid? itemTypeId, float quantity)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
        public Guid? ItemTypeId { get; }
        public float Quantity { get; }
    }
}