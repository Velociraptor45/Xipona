using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public static class TemporaryItemCreationExtensions
    {
        public static StoreItem ToStoreItem(this TemporaryItemCreation model)
        {
            return new StoreItem(
                new StoreItemId(model.ClientSideId),
                model.Name,
                false,
                string.Empty,
                true,
                QuantityType.Unit,
                1,
                QuantityTypeInPacket.Unit,
                null,
                null,
                new List<StoreItemAvailability>() { model.Availability },
                null);
        }
    }
}