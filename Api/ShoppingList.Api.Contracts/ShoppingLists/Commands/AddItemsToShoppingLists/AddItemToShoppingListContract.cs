using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemsToShoppingLists
{
    public class AddItemToShoppingListContract
    {
        public AddItemToShoppingListContract(Guid itemId, Guid? itemTypeId, Guid storeId, float quantity)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            StoreId = storeId;
            Quantity = quantity;
        }

        public Guid ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
        public Guid StoreId { get; set; }
        public float Quantity { get; set; }
    }
}