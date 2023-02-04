namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.ShoppingLists
{
    public class AddItemWithTypeToShoppingListRequest
    {
        public AddItemWithTypeToShoppingListRequest(Guid requestId, Guid shoppingListId, Guid itemId, Guid itemTypeId,
            float quantity, Guid? sectionId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
            SectionId = sectionId;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public Guid ItemId { get; }
        public Guid ItemTypeId { get; }
        public float Quantity { get; }
        public Guid? SectionId { get; }
    }
}