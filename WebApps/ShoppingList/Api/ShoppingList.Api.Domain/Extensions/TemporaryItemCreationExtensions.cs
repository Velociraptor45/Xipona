using ShoppingList.Api.Domain.Commands.CreateTemporaryItem;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Extensions
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
                new List<StoreItemAvailability>() { model.Availability });
        }
    }
}