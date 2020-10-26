using System;
using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class StoreItemAvailabilityConverter
    {
        public static Entities.AvailableAt ToEntity(this Models.StoreItemAvailability model, Models.StoreItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            return new Entities.AvailableAt()
            {
                StoreId = model.StoreId.Value,
                Price = model.Price,
                ItemId = itemId.Value
            };
        }
    }
}