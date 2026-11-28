using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Models.Factories;
using Xipona.Api.Repositories.Items.Entities;

namespace Xipona.Api.Repositories.Items.Converters.ToDomain;

public class ItemTypeConverter : IToDomainConverter<Entities.ItemType, IItemType>
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IToDomainConverter<ItemTypeAvailableAt, ItemAvailability> _itemTypeAvailabilityConverter;

    public ItemTypeConverter(IItemTypeFactory itemTypeFactory,
        IToDomainConverter<ItemTypeAvailableAt, ItemAvailability> itemTypeAvailabilityConverter)
    {
        _itemTypeFactory = itemTypeFactory;
        _itemTypeAvailabilityConverter = itemTypeAvailabilityConverter;
    }

    public IItemType ToDomain(Entities.ItemType source)
    {
        var predecessorId = source.PredecessorId is null
            ? (ItemTypeId?)null
            : new ItemTypeId(source.PredecessorId.Value);

        var typeAvailabilities = _itemTypeAvailabilityConverter.ToDomain(source.AvailableAt);
        return _itemTypeFactory.Create(new ItemTypeId(source.Id), new ItemTypeName(source.Name), typeAvailabilities,
            predecessorId, source.IsDeleted, source.CreatedAt);
    }
}