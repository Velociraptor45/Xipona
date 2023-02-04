using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class AddItemToShoppingListRequest : IApiRequest
    {
        public AddItemToShoppingListRequest(Guid requestId, Guid shoppingListId, ShoppingListItemId itemId, float quantity,
            Guid? sectionId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            Quantity = quantity;
            SectionId = sectionId;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public ShoppingListItemId ItemId { get; }
        public float Quantity { get; }
        public Guid? SectionId { get; }
    }
}