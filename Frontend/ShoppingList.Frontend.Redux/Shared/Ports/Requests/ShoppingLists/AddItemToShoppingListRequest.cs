namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class AddItemToShoppingListRequest
    {
        public AddItemToShoppingListRequest(Guid requestId, Guid shoppingListId, Guid itemId, float quantity,
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
        public Guid ItemId { get; }
        public float Quantity { get; }
        public Guid? SectionId { get; }
    }
}