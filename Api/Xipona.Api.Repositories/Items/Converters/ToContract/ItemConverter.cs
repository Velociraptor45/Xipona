using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Repositories.Items.Entities;
using Item = ProjectHermes.Xipona.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.Xipona.Api.Repositories.Items.Converters.ToContract;

public class ItemConverter : IToContractConverter<IItem, Entities.Item>
{
    public Item ToContract(IItem source)
    {
        return new Item
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Comment = source.Comment.Value,
            IsTemporary = source.IsTemporary,
            QuantityType = source.ItemQuantity.Type.ToInt(),
            QuantityInPacket = source.ItemQuantity.InPacket?.Quantity,
            QuantityTypeInPacket = source.ItemQuantity.InPacket?.Type.ToInt(),
            ItemCategoryId = source.ItemCategoryId,
            ManufacturerId = source.ManufacturerId,
            CreatedFrom = source.TemporaryId,
            CreatedAt = source.CreatedAt,
            AvailableAt = source.Availabilities.Select(av => ToAvailableAt(av, source)).ToList(),
            ItemTypes = source.ItemTypes.Select(type => ToItemType(type, source)).ToList(),
            UpdatedOn = source.UpdatedOn,
            PredecessorId = source.PredecessorId,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }

    private static AvailableAt ToAvailableAt(ItemAvailability availability, IItem source)
    {
        return new AvailableAt
        {
            StoreId = availability.StoreId,
            Price = availability.Price,
            ItemId = source.Id,
            DefaultSectionId = availability.DefaultSectionId
        };
    }

    private static Entities.ItemType ToItemType(IItemType itemType, IItem source)
    {
        return new Entities.ItemType
        {
            Id = itemType.Id,
            ItemId = source.Id,
            Name = itemType.Name,
            CreatedAt = itemType.CreatedAt,
            AvailableAt = itemType.Availabilities.Select(av => ToItemTypeAvailableAt(av, itemType)).ToList(),
            PredecessorId = itemType.PredecessorId,
            IsDeleted = itemType.IsDeleted
        };
    }

    private static ItemTypeAvailableAt ToItemTypeAvailableAt(ItemAvailability availability, IItemType itemType)
    {
        return new ItemTypeAvailableAt
        {
            StoreId = availability.StoreId,
            Price = availability.Price,
            ItemTypeId = itemType.Id,
            DefaultSectionId = availability.DefaultSectionId
        };
    }
}