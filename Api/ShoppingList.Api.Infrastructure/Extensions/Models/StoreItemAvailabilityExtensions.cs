using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class StoreItemAvailabilityExtensions
    {
        public static Infrastructure.Entities.AvailableAt ToEntity(this IStoreItemAvailability model, StoreItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            return new Infrastructure.Entities.AvailableAt()
            {
                StoreId = model.StoreId.Value,
                Price = model.Price,
                ItemId = itemId.Actual?.Value ?? 0,
                DefaultSectionId = model.DefaultSection.Id.Value
            };
        }
    }
}