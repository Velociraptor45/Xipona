using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems
{
    public class StoreItemAvailabilityContractConverter :
        IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
    {
        private readonly IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter;
        private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter;

        public StoreItemAvailabilityContractConverter(
            IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter,
            IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
        {
            this.storeItemStoreContractConverter = storeItemStoreContractConverter;
            this.storeSectionContractConverter = storeSectionContractConverter;
        }

        public StoreItemAvailabilityContract ToContract(StoreItemAvailabilityReadModel source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new StoreItemAvailabilityContract(
                storeItemStoreContractConverter.ToContract(source.Store),
                source.Price,
                storeSectionContractConverter.ToContract(source.DefaultSection));
        }
    }
}