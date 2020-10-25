using ShoppingList.Domain.Models;
using ShoppingList.Infrastructure.Entities;
using System;

namespace ShoppingList.Infrastructure.Converters
{
    public static class StoreItemAvailabilityConverter
    {
        public static AvailableAt ToEntity(this StoreItemAvailability model, StoreItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            return new AvailableAt()
            {
                StoreId = model.StoreId.Value,
                Price = model.Price,
                ItemId = itemId.Value
            };
        }
    }
}