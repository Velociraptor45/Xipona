using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList
{
    public class AddTemporaryItemToShoppingListContract
    {
        public AddTemporaryItemToShoppingListContract(string itemName, int quantityType,
            float quantity, float price, Guid sectionId, Guid temporaryItemId)
        {
            ItemName = itemName;
            QuantityType = quantityType;
            Quantity = quantity;
            Price = price;
            SectionId = sectionId;
            TemporaryItemId = temporaryItemId;
        }

        public string ItemName { get; set; }
        public int QuantityType { get; set; }
        public float Quantity { get; set; }
        public float Price { get; set; }
        public Guid SectionId { get; set; }
        public Guid TemporaryItemId { get; set; }
    }
}