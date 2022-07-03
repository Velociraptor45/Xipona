using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemStoreContractConverter :
    IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract>
{
    private readonly IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> _storeItemSectionContractConverter;

    public StoreItemStoreContractConverter(
        IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeItemSectionContractConverter)
    {
        _storeItemSectionContractConverter = storeItemSectionContractConverter;
    }

    public StoreItemStoreContract ToContract(StoreItemStoreReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new StoreItemStoreContract(
            source.Id.Value,
            source.Name.Value,
            _storeItemSectionContractConverter.ToContract(source.Sections));
    }
}