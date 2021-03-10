using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using System;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToDomain
{
    public class StoreItemAvailabilityConverter : IToDomainConverter<AvailableAt, IStoreItemAvailability>
    {
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IToDomainConverter<Store, IStoreItemStore> storeItemStoreConverter;

        public StoreItemAvailabilityConverter(IStoreItemAvailabilityFactory storeItemAvailabilityFactory,
            IToDomainConverter<Store, IStoreItemStore> storeItemStoreConverter)
        {
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemStoreConverter = storeItemStoreConverter;
        }

        public IStoreItemAvailability ToDomain(AvailableAt source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            IStoreItemStore store = storeItemStoreConverter.ToDomain(source.Store);

            return storeItemAvailabilityFactory.Create(
                store,
                source.Price,
                new StoreItemSectionId(source.DefaultSectionId));
        }
    }
}