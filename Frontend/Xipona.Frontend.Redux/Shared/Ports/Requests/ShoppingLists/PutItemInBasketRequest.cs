using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class PutItemInBasketRequest : IApiRequest
    {
        public PutItemInBasketRequest(Guid requestId, Guid shoppingListId, ShoppingListItemId itemId, Guid? itemTypeId,
            string itemName)
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