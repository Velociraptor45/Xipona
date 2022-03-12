using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.StoreItems;

public class ItemTypeContractConverter : IToContractConverter<ItemTypeReadModel, ItemTypeContract>
{
    private readonly IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
        _storeItemAvailabilityContractConverter;

    public ItemTypeContractConverter(
        IToContractConverter<StoreItemAvailabilityReadModel, StoreItemAvailabilityContract>
            storeItemAvailabilityContractConverter)
    {
        _storeItemAvailabilityContractConverter = storeItemAvailabilityContractConverter;
    }

    public ItemTypeContract ToContract(ItemTypeReadModel source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new ItemTypeContract()
        {
            Id = source.Id.Value,
            Name = source.Name,
            Availabilities = _storeItemAvailabilityContractConverter.ToContract(source.Availabilities)
        };
    }
}