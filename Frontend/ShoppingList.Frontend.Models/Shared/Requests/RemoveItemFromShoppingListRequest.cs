using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class RemoveItemFromShoppingListRequest : IApiRequest
    {
        public RemoveItemFromShoppingListRequest(Guid requestId, Guid shoppingListId, ItemId itemId, Guid? itemTypeId)
        {
            RequestId = requestId;
            ShoppingListId = shoppingListId;
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        public Guid RequestId { get; }
        public Guid ShoppingListId { get; }
        public ItemId ItemId { get; }
        public Guid? ItemTypeId { get; }
    }
}