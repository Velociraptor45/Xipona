using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public class TemporaryItemCreationService : ITemporaryItemCreationService
    {
        public ShoppingListItem Create(string name, float price)
        {
            // ugly
            var quantityType = new QuantityType(0, "", 1, "€", "x", 1);
            var quantityTypeInPacket = new QuantityTypeInPacket(0, "", "");
            var offlineId = ShoppingListItemId.FromOfflineId(Guid.NewGuid());
            return new ShoppingListItem(
                offlineId,
                typeId: null,
                name,
                isTemporary: true,
                price,
                quantityType,
                quantityInPacket: 1,
                quantityTypeInPacket,
                itemCategory: "",
                manufacturer: "",
                isInBasket: false,
                quantity: 1);
        }
    }
}