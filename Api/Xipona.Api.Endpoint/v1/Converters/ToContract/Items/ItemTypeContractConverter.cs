using ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Items;

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
            source.Id,
            source.Name,
            _itemAvailabilityContractConverter.ToContract(source.Availabilities));
    }
}