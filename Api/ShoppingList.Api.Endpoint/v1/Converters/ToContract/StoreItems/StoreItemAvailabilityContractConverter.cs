using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemAvailabilityContractConverter :
    IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
{
    private readonly IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> _storeItemStoreContractConverter;
    private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> _storeSectionContractConverter;

    public StoreItemAvailabilityContractConverter(
        IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter,
        IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
    {
        _storeItemStoreContractConverter = storeItemStoreContractConverter;
        _storeSectionContractConverter = storeSectionContractConverter;
    }

    public StoreItemAvailabilityContract ToContract(StoreItemAvailabilityReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreItemAvailabilityContract(
            _storeItemStoreContractConverter.ToContract(source.Store),
            source.Price,
            _storeSectionContractConverter.ToContract(source.DefaultSection));
    }
}