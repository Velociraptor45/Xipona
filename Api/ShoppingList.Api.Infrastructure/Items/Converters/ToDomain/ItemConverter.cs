using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Items.Converters.ToDomain;

public class ItemConverter : IToDomainConverter<Item, IItem>
{
    private readonly IItemFactory _itemFactory;
    private readonly IToDomainConverter<Entities.ItemType, IItemType> _itemTypeConverter;
    private readonly IToDomainConverter<AvailableAt, IItemAvailability> _itemAvailabilityConverter;

    public ItemConverter(IItemFactory itemFactory,
        IToDomainConverter<Entities.ItemType, IItemType> itemTypeConverter,
        IToDomainConverter<AvailableAt, IItemAvailability> itemAvailabilityConverter)
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

        if (source.ItemTypes.Any())
        {
            var itemTypes = _itemTypeConverter.ToDomain(source.ItemTypes);

            if (itemCategoryId is null)
                throw new InvalidOperationException("ItemCategoryId mustn't be null for an item with types.");

            return _itemFactory.Create(
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
                source.UpdatedOn);
        }

        List<IItemAvailability> availabilities = _itemAvailabilityConverter.ToDomain(source.AvailableAt)
            .ToList();

        return _itemFactory.Create(
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
            source.UpdatedOn);
    }
}