using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists
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