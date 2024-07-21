using System;

namespace ProjectHermes.Xipona.Api.Contracts.ShoppingLists.Commands.AddTemporaryItemToShoppingList
{
    public class AddTemporaryItemToShoppingListContract
    {
        public AddTemporaryItemToShoppingListContract(string itemName, int quantityType,
            float quantity, decimal price, Guid sectionId, Guid temporaryItemId)
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
        public decimal Price { get; set; }
        public Guid SectionId { get; set; }
        public Guid TemporaryItemId { get; set; }
    }
}