using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems
{
    public class StoreItemStoreContractConverter :
        IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract>
    {
        private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter;

        public StoreItemStoreContractConverter(
            IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
        {
            this.storeSectionContractConverter = storeSectionContractConverter;
        }

        public StoreItemStoreContract ToContract(StoreItemStoreReadModel source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new StoreItemStoreContract(
                source.Id.Value,
                source.Name,
                storeSectionContractConverter.ToContract(source.Sections));
        }
    }
}