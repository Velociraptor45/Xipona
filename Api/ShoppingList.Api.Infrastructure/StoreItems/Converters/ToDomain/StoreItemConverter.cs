using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToDomain;

public class StoreItemConverter : IToDomainConverter<Item, IStoreItem>
{
    private readonly IStoreItemFactory _storeItemFactory;
    private readonly IToDomainConverter<Entities.ItemType, IItemType> _itemTypeConverter;
    private readonly IToDomainConverter<AvailableAt, IStoreItemAvailability> _storeItemAvailabilityConverter;

    public StoreItemConverter(IStoreItemFactory storeItemFactory,
        IToDomainConverter<Entities.ItemType, IItemType> itemTypeConverter,
        IToDomainConverter<AvailableAt, IStoreItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemFactory = storeItemFactory;
        _itemTypeConverter = itemTypeConverter;
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public IStoreItem ToDomain(Item source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        IStoreItem? predecessor = source.PredecessorId is null ? null : ToDomain(source.Predecessor!);

        var itemCategoryId = source.ItemCategoryId.HasValue
            ? new ItemCategoryId(source.ItemCategoryId.Value)
            : (ItemCategoryId?)null;
        var manufacturerId = source.ManufacturerId.HasValue
            ? new ManufacturerId(source.ManufacturerId.Value)
            : (ManufacturerId?)null;
        var temporaryId = source.CreatedFrom.HasValue
            ? new TemporaryItemId(source.CreatedFrom.Value)
            : (TemporaryItemId?)null;

        if (source.ItemTypes.Any())
        {
            var itemTypes = _itemTypeConverter.ToDomain(source.ItemTypes);

            if (itemCategoryId is null)
                throw new InvalidOperationException("ItemCategoryId mustn't be null for an item with types.");

            return _storeItemFactory.Create(
                new ItemId(source.Id),
                new ItemName(source.Name),
                source.Deleted,
                new Comment(source.Comment),
                source.QuantityType.ToEnum<QuantityType>(),
                source.QuantityInPacket,
                source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
                itemCategoryId.Value,
                manufacturerId,
                predecessor,
                itemTypes);
        }

        List<IStoreItemAvailability> availabilities = _storeItemAvailabilityConverter.ToDomain(source.AvailableAt)
            .ToList();

        return _storeItemFactory.Create(
            new ItemId(source.Id),
            new ItemName(source.Name),
            source.Deleted,
            new Comment(source.Comment),
            source.IsTemporary,
            source.QuantityType.ToEnum<QuantityType>(),
            source.QuantityInPacket,
            source.QuantityTypeInPacket.ToEnum<QuantityTypeInPacket>(),
            itemCategoryId,
            manufacturerId,
            predecessor,
            availabilities,
            temporaryId);
    }
}