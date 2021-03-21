using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class AddItemToShoppingListRequest : IApiRequest
    {
        public AddItemToShoppingListRequest(Guid requestId, int shoppingListId, ItemId itemId, float quantity,
            int? sectionId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            Quantity = quantity;
            SectionId = sectionId;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public ItemId ItemId { get; }
        public float Quantity { get; }
        public int? SectionId { get; }
    }
}