using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class StoreItemAvailabilityContractConverter :
    IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
{
    private readonly IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> _storeItemStoreContractConverter;
    private readonly IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> _storeSectionContractConverter;

    public StoreItemAvailabilityContractConverter(
        IToContractConverter<StoreItemStoreReadModel, StoreItemStoreContract> storeItemStoreContractConverter,
        IToContractConverter<StoreItemSectionReadModel, StoreItemSectionContract> storeSectionContractConverter)
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
            source.Price.Value,
            _storeSectionContractConverter.ToContract(source.DefaultSection));
    }
}