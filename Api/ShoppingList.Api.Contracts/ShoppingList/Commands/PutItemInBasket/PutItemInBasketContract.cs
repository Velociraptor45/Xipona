using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket
{
    public class PutItemInBasketContract
    {
        public Guid ShoppingListId { get; set; }
        public ItemIdContract ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
    }
}