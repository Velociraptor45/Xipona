using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Converters.ToEntity;

public class ItemConverter : IToEntityConverter<IStoreItem, Item>
{
    public Item ToEntity(IStoreItem source)
    {
        if (source is null)
            throw new System.ArgumentNullException(nameof(source));

        return new Item
        {
            Id = source.Id.Value,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Comment = source.Comment,
            IsTemporary = source.IsTemporary,
            QuantityType = source.QuantityType.ToInt(),
            QuantityInPacket = source.QuantityInPacket,
            QuantityTypeInPacket = source.QuantityTypeInPacket.ToInt(),
            ItemCategoryId = source.ItemCategoryId?.Value,
            ManufacturerId = source.ManufacturerId?.Value,
            CreatedFrom = source.TemporaryId?.Value,
            AvailableAt = source.Availabilities.Select(av => ToAvailableAt(av, source)).ToList(),
            ItemTypes = source.ItemTypes.Select(type => ToItemType(type, source)).ToList(),
            PredecessorId = source.Predecessor?.Id.Value
        };
    }

    private static AvailableAt ToAvailableAt(IStoreItemAvailability availability, IStoreItem source)
    {
        return new AvailableAt
        {
            StoreId = availability.StoreId.Value,
            Price = availability.Price,
            ItemId = source.Id.Value,
            DefaultSectionId = availability.DefaultSectionId.Value
        };
    }

    private static Entities.ItemType ToItemType(IItemType itemType, IStoreItem source)
    {
        return new Entities.ItemType
        {
            Id = itemType.Id.Value,
            ItemId = source.Id.Value,
            Name = itemType.Name,
            AvailableAt = itemType.Availabilities.Select(av => ToItemTypeAvailableAt(av, itemType)).ToList(),
            PredecessorId = itemType.Predecessor?.Id.Value
        };
    }

    private static ItemTypeAvailableAt ToItemTypeAvailableAt(IStoreItemAvailability availability, IItemType itemType)
    {
        return new ItemTypeAvailableAt
        {
            StoreId = availability.StoreId.Value,
            Price = availability.Price,
            ItemTypeId = itemType.Id.Value,
            DefaultSectionId = availability.DefaultSectionId.Value
        };
    }
}