using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;

public class ItemFactory : IItemFactory
{
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IDateTimeService _dateTimeService;

    public ItemFactory(IItemTypeFactory itemTypeFactory, IDateTimeService dateTimeService)
    {
        _itemTypeFactory = itemTypeFactory;
        _dateTimeService = dateTimeService;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        ItemId? predecessorId, IEnumerable<ItemAvailability> availabilities, TemporaryItemId? temporaryId,
        DateTimeOffset? updatedOn, DateTimeOffset createdAt)
    {
        var item = new Item(
            id,
            name,
            isDeleted,
            comment,
            isTemporary,
            itemQuantity,
            itemCategoryId,
            manufacturerId,
            availabilities,
            temporaryId,
            updatedOn,
            predecessorId,
            createdAt);

        return item;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, ItemId? predecessorId,
        IEnumerable<IItemType> itemTypes, DateTimeOffset? updatedOn, DateTimeOffset createdAt)
    {
        var item = new Item(
            id,
            name,
            isDeleted,
            comment,
            itemQuantity,
            itemCategoryId,
            manufacturerId,
            new ItemTypes(itemTypes, _itemTypeFactory),
            updatedOn,
            predecessorId,
            createdAt);

        return item;
    }

    public IItem Create(ItemCreation itemCreation)
    {
        return new Item(
            ItemId.New,
            itemCreation.Name,
            false,
            itemCreation.Comment,
            false,
            itemCreation.ItemQuantity,
            itemCreation.ItemCategoryId,
            itemCreation.ManufacturerId,
            itemCreation.Availabilities,
            null,
            null,
            null,
            _dateTimeService.UtcNow);
    }

    public IItem CreateTemporary(ItemName name, QuantityType quantityType, StoreId storeId, Price price,
        SectionId defaultSectionId, TemporaryItemId temporaryItemId)
    {
        var itemQuantity = quantityType switch
        {
            QuantityType.Weight => new ItemQuantity(quantityType, null),
            QuantityType.Unit => new ItemQuantity(quantityType, new ItemQuantityInPacket(new Quantity(1), QuantityTypeInPacket.Unit)),
            _ => throw new ArgumentOutOfRangeException($"QuantityType {quantityType} is not supported for temporary items.")
        };

        return new Item(
            ItemId.New,
            name,
            false,
            Comment.Empty,
            true,
            itemQuantity,
            null,
            null,
            new List<ItemAvailability>
            {
                new ItemAvailability(storeId, price, defaultSectionId)
            },
            temporaryItemId,
            null,
            null,
            _dateTimeService.UtcNow);
    }

    public IItem CreateNew(ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, ItemId? predecessorId,
        IEnumerable<IItemType> itemTypes)
    {
        var item = new Item(
            ItemId.New,
            name,
            isDeleted: false,
            comment,
            itemQuantity,
            itemCategoryId,
            manufacturerId,
            new ItemTypes(itemTypes, _itemTypeFactory),
            null,
            predecessorId,
            _dateTimeService.UtcNow);

        return item;
    }
}