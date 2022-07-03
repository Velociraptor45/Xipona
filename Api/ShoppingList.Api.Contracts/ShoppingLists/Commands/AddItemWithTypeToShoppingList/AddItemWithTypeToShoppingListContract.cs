using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemWithTypeToShoppingList
{
    public class AddItemWithTypeToShoppingListContract
    {
        public AddItemWithTypeToShoppingListContract(Guid? sectionId, float quantity)
        {
            SectionId = sectionId;
            Quantity = quantity;
        }

        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}