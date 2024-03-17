using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromShoppingList
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