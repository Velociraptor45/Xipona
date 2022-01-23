using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class AddItemWithTypeToShoppingListRequest
    {
        public AddItemWithTypeToShoppingListRequest(Guid requestId, int shoppingListId, int itemId, int itemTypeId,
            float quantity, int? sectionId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
            SectionId = sectionId;
        }

        public Guid RequestId { get; }
        public int ShoppingListId { get; }
        public int ItemId { get; }
        public int ItemTypeId { get; }
        public float Quantity { get; }
        public int? SectionId { get; }
    }
}