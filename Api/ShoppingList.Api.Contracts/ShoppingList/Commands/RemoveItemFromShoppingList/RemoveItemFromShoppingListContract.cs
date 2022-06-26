using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListContract
    {
        public RemoveItemFromShoppingListContract(ItemIdContract itemId, Guid? itemTypeId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}