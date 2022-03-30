using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListContract
    {
        public ItemIdContract ItemId { get; set; }
        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}