using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class TemporaryItemCreationConverter : IToDomainConverter<CreateTemporaryItemContract, TemporaryItemCreation>
    {
        private readonly IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter;

        public TemporaryItemCreationConverter(
            IToDomainConverter<ItemAvailabilityContract, ShortAvailability> shortAvailabilityConverter)
        {
            this.shortAvailabilityConverter = shortAvailabilityConverter;
        }

        public TemporaryItemCreation ToDomain(CreateTemporaryItemContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new TemporaryItemCreation(
                source.ClientSideId,
                source.Name,
                shortAvailabilityConverter.ToDomain(source.Availability));
        }
    }
}