using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class StoreItemAvailabilityConverter : IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability>
    {
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;

        public StoreItemAvailabilityConverter(IStoreItemAvailabilityFactory storeItemAvailabilityFactory)
        {
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
        }

        public IStoreItemAvailability ToDomain(ItemAvailabilityContract source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return storeItemAvailabilityFactory.Create(
                new StoreId(source.StoreId),
                source.Price,
                new SectionId(source.DefaultSectionId));
        }
    }
}