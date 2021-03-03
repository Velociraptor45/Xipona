using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItem
{
    public class StoreItemStoreContractConverter :
        IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract>
    {
        private readonly IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter;

        public StoreItemStoreContractConverter(
            IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter)
        {
            this.storeItemSectionContractConverter = storeItemSectionContractConverter;
        }

        public StoreItemStoreContract ToContract(StoreItemStoreReadModel source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new StoreItemStoreContract(
                source.Id.Value,
                source.Name,
                storeItemSectionContractConverter.ToContract(source.Sections));
        }
    }
}