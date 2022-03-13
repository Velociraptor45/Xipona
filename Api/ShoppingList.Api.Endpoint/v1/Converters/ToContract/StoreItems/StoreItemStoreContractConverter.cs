using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemStoreContractConverter :
    IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract>
{
    private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> _storeSectionContractConverter;

    public StoreItemStoreContractConverter(
        IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
    {
        _storeSectionContractConverter = storeSectionContractConverter;
    }

    public StoreItemStoreContract ToContract(StoreItemStoreReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreItemStoreContract(
            source.Id.Value,
            source.Name,
            _storeSectionContractConverter.ToContract(source.Sections));
    }
}