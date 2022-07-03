using ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListContract
    {
        public AddItemToShoppingListContract(ItemIdContract itemId, Guid? sectionId, float quantity)
        {
            ItemId = itemId;
            SectionId = sectionId;
            Quantity = quantity;
        }

        public ItemIdContract ItemId { get; set; }
        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}