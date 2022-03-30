using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList
{
    public class AddItemWithTypeToShoppingListContract
    {
        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}