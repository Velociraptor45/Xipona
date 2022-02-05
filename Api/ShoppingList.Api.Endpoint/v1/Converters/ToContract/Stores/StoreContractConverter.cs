using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

public class ActiveStoreContractConverter : IToContractConverter<StoreReadModel, ActiveStoreContract>
{
    private readonly IToContractConverter<StoreSectionReadModel, StoreSectionContract> _storeSectionContractConverter;

    public ActiveStoreContractConverter(
        IToContractConverter<StoreSectionReadModel, StoreSectionContract> storeSectionContractConverter)
    {
        _storeSectionContractConverter = storeSectionContractConverter;
    }

    public ActiveStoreContract ToContract(StoreReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ActiveStoreContract(
            source.Id.Value,
            source.Name,
            source.ItemCount,
            _storeSectionContractConverter.ToContract(source.Sections));
    }
}