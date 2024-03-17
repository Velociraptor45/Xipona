using ProjectHermes.Xipona.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.Xipona.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToDomain.Items;

public class CreateItemTypeConverter : IToDomainConverter<CreateItemTypeContract, IItemType>
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IToDomainConverter<ItemAvailabilityContract, ItemAvailability> _itemAvailabilityConverter;

    public CreateItemTypeConverter(IItemTypeFactory itemTypeFactory,
        IToDomainConverter<ItemAvailabilityContract, ItemAvailability> itemAvailabilityConverter)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public IItemType ToDomain(CreateItemTypeContract source)
    {
        return _itemTypeFactory.CreateNew(
            new ItemTypeName(source.Name),
            _itemAvailabilityConverter.ToDomain(source.Availabilities),
            null);
    }
}