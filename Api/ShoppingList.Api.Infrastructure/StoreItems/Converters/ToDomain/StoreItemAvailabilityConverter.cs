using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain
{
    public class StoreItemAvailabilityConverter : IToDomainConverter<AvailableAt, IStoreItemAvailability>
    {
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;

        public StoreItemAvailabilityConverter(IStoreItemAvailabilityFactory storeItemAvailabilityFactory)
        {
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
        }

        public IStoreItemAvailability ToDomain(AvailableAt source)
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