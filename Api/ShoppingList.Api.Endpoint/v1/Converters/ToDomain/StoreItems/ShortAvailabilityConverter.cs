using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class ShortAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, ShortAvailability>
    {
        public ShortAvailability ToDomain(ItemAvailabilityContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ShortAvailability(
                new StoreItemStoreId(source.StoreId),
                source.Price,
                new StoreItemSectionId(source.DefaultSectionId));
        }
    }
}