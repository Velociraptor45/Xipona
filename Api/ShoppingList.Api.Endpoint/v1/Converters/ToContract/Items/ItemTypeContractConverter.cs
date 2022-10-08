using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Items;

public class ItemTypeContractConverter : IToContractConverter<ItemTypeReadModel, ItemTypeContract>
{
    private readonly IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract>
        _itemAvailabilityContractConverter;

    public ItemTypeContractConverter(
        IToContractConverter<ItemAvailabilityReadModel, ItemAvailabilityContract> itemAvailabilityContractConverter)
    {
        _itemAvailabilityContractConverter = itemAvailabilityContractConverter;
    }

    public ItemTypeContract ToContract(ItemTypeReadModel source)
    {
        return new ItemTypeContract(
            source.Id.Value,
            source.Name.Value,
            _itemAvailabilityContractConverter.ToContract(source.Availabilities));
    }
}