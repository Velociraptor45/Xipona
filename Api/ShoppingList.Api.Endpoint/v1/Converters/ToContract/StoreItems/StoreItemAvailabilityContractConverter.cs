using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems
{
    public class StoreItemAvailabilityContractConverter :
        IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
    {
        private readonly IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter;
        private readonly IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter;

        public StoreItemAvailabilityContractConverter(
            IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter,
            IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter)
        {
            this.storeItemStoreContractConverter = storeItemStoreContractConverter;
            this.storeItemSectionContractConverter = storeItemSectionContractConverter;
        }

        public StoreItemAvailabilityContract ToContract(StoreItemAvailabilityReadModel source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new StoreItemAvailabilityContract(
                storeItemStoreContractConverter.ToContract(source.Store),
                source.Price,
                storeItemSectionContractConverter.ToContract(source.DefaultSection));
        }
    }
}