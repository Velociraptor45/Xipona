using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class RemoveItemFromBasketRequest : IApiRequest
    {
        public RemoveItemFromBasketRequest(Guid requestId, Guid shoppingListId, ShoppingListItemId itemId,
            Guid? itemTypeId, string itemName)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            ItemName = itemName;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
        public Guid? ItemTypeId { get; }
        public string ItemName { get; }
    }
}