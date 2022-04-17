using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists
{
    public class AddItemToShoppingListRequest : IApiRequest
    {
        public AddItemToShoppingListRequest(Guid requestId, Guid shoppingListId, ItemId itemId, float quantity,
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
        public ItemId ItemId { get; }
        public float Quantity { get; }
        public Guid? SectionId { get; }
    }
}