using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index.Services
{
    public class TemporaryItemCreationService : ITemporaryItemCreationService
    {
        public ShoppingListItem Create(string name)
        {
            // ugly
            var quantityType = new QuantityType(0, "", 1, "€", "x", 1);
            var quantityTypeInPacket = new QuantityTypeInPacket(0, "", "");
            return new ShoppingListItem(new ItemId(Guid.NewGuid()), null, name, true, 1f,
                quantityType, 1, quantityTypeInPacket, "", "", false, 1);
        }
    }
}