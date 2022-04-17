using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public class TemporaryItemCreationService : ITemporaryItemCreationService
    {
        public ShoppingListItem Create(string name)
        {
            // ugly
            var quantityType = new QuantityType(0, "", 1, "€", "x", 1);
            var quantityTypeInPacket = new QuantityTypeInPacket(0, "", "");
            return new ShoppingListItem(ItemId.FromOfflineId(Guid.NewGuid()), null, name, true, 1f,
                quantityType, 1, quantityTypeInPacket, "", "", false, 1);
        }
    }
}