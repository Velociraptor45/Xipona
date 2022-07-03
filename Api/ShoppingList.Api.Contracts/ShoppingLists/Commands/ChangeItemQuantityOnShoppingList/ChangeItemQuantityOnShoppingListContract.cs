using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListContract
    {
        public ChangeItemQuantityOnShoppingListContract(ItemIdContract itemId, Guid? itemTypeId, float quantity)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Quantity = quantity;
        }

        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
        public float Quantity { get; set; }
    }
}