using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListContract
    {
        public AddItemToShoppingListContract(Guid itemId, Guid? sectionId, float quantity)
        {
            ItemId = itemId;
            SectionId = sectionId;
            Quantity = quantity;
        }

        public Guid ItemId { get; set; }
        public Guid? SectionId { get; set; }
        public float Quantity { get; set; }
    }
}