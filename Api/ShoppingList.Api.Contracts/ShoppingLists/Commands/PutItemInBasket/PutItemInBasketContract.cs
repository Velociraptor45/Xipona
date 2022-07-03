using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketContract
    {
        public PutItemInBasketContract(ItemIdContract itemId, Guid? itemTypeId)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
        }

        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}