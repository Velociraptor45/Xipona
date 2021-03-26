using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;
using System;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores
{
    public class ActiveStoreContractConverter : IToContractConverter<StoreReadModel, ActiveStoreContract>
    {
        private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter;

        public ActiveStoreContractConverter(
            IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
        {
            this.storeSectionContractConverter = storeSectionContractConverter;
        }

        public ActiveStoreContract ToContract(StoreReadModel source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ActiveStoreContract(
                source.Id.Value,
                source.Name,
                source.ItemCount,
                storeSectionContractConverter.ToContract(source.Sections));
        }
    }
}