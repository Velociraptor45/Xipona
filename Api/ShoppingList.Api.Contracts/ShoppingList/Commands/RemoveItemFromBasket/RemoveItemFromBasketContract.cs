using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketContract
    {
        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}