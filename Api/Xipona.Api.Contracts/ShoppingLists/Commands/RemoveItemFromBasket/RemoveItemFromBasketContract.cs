using ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketContract
    {
        public RemoveItemFromBasketContract(ItemIdContract itemId, Guid? itemTypeId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}