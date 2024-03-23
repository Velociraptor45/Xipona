using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToDomain;

public class ItemConverter : IToDomainConverter<Entities.Item, IItem>
{
    private readonly IItemFactory _itemFactory;
    private readonly IToDomainConverter<Entities.ItemType, IItemType> _itemTypeConverter;
    private readonly IToDomainConverter<AvailableAt, ItemAvailability> _itemAvailabilityConverter;

    public ItemConverter(IItemFactory itemFactory,
        IToDomainConverter<Entities.ItemType, IItemType> itemTypeConverter,
        IToDomainConverter<AvailableAt, ItemAvailability> itemAvailabilityConverter)
    {
        _itemFactory = itemFactory;
        _itemTypeConverter = itemTypeConverter;
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public IItem ToDomain(Item source)
    {
        var predecessorId = source.PredecessorId is null ? (ItemId?)null : new ItemId(source.PredecessorId.Value);
        var itemCategoryId = source.ItemCategoryId.HasValue
            ? new ItemCategoryId(source.ItemCategoryId.Value)
            : (ItemCategoryId?)null;
        var manufacturerId = source.ManufacturerId.HasValue
            ? new ManufacturerId(source.ManufacturerId.Value)
            : (ManufacturerId?)null;
        var temporaryId = source.CreatedFrom.HasValue
            ? new TemporaryItemId(source.CreatedFrom.Value)
            : (TemporaryItemId?)null;

        ItemQuantityInPacket? itemQuantityInPacket = null;
        if (source.QuantityInPacket is null)
        {
            if (source.QuantityTypeInPacket is not null)
                throw new InvalidOperationException(
                    $"Invalid data state for item {source.Id}: QuantityInPacket is null but QuantityTypeInPacket isn't.");
        }
        else
        {
            if (source.QuantityTypeInPacket is null)
                throw new InvalidOperationException(
                    $"Invalid data state for item {source.Id}: QuantityInPacket isn't null but QuantityTypeInPacket is.");

            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(source.QuantityInPacket.Value),
                source.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        AggregateRoot item;
        if (source.ItemTypes.Count != 0)
        {
            var itemTypes = _itemTypeConverter.ToDomain(source.ItemTypes);

            if (itemCategoryId is null)
                throw new InvalidOperationException("ItemCategoryId mustn't be null for an item with types.");

            item = (AggregateRoot)_itemFactory.Create(
                new ItemId(source.Id),
                new ItemName(source.Name),
                source.Deleted,
                new Comment(source.Comment),
                new ItemQuantity(
                    source.QuantityType.ToEnum<QuantityType>(),
                    itemQuantityInPacket),
                itemCategoryId.Value,
                manufacturerId,
                predecessorId,
                itemTypes,
                source.UpdatedOn,
                source.CreatedAt);
        }
        else
        {
            List<ItemAvailability> availabilities = _itemAvailabilityConverter.ToDomain(source.AvailableAt)
                .ToList();

            item = (AggregateRoot)_itemFactory.Create(
                new ItemId(source.Id),
                new ItemName(source.Name),
                source.Deleted,
                new Comment(source.Comment),
                source.IsTemporary,
                new ItemQuantity(
                    source.QuantityType.ToEnum<QuantityType>(),
                    itemQuantityInPacket),
                itemCategoryId,
                manufacturerId,
                predecessorId,
                availabilities,
                temporaryId,
                source.UpdatedOn,
                source.CreatedAt);
        }

        item.EnrichWithRowVersion(source.RowVersion);
        return (item as IItem)!;
    }
}