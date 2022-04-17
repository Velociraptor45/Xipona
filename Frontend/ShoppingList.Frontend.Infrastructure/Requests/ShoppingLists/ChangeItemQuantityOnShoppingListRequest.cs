using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists
{
    public class ChangeItemQuantityOnShoppingListRequest : IApiRequest
    {
        public ChangeItemQuantityOnShoppingListRequest(Guid requestId, Guid shoppingListId, ItemId itemId,
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
        public ItemId ItemId { get; }
        public Guid? ItemTypeId { get; }
        public float Quantity { get; }
    }
}