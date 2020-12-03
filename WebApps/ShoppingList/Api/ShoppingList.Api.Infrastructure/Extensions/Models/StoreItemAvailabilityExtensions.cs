using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreItemAvailabilityExtensions
    {
        public static Infrastructure.Entities.AvailableAt ToEntity(this StoreItemAvailability model, StoreItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            return new Infrastructure.Entities.AvailableAt()
            {
                StoreId = model.StoreId.Value,
                Price = model.Price,
                ItemId = itemId.Actual?.Value ?? 0
            };
        }
    }
}