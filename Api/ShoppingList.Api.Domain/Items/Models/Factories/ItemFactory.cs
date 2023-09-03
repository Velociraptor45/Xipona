using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public class ItemFactory : IItemFactory
{
    private readonly IItemTypeFactory _itemTypeFactory;

    public ItemFactory(IItemTypeFactory itemTypeFactory)
    {
        _itemTypeFactory = itemTypeFactory;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        ItemId? predecessorId, IEnumerable<ItemAvailability> availabilities, TemporaryItemId? temporaryId,
        DateTimeOffset? updatedOn)
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
            predecessorId);

        return item;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, ItemId? predecessorId,
        IEnumerable<IItemType> itemTypes, DateTimeOffset? updatedOn)
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
            predecessorId);

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
            null);
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
            null);
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
            predecessorId);

        return item;
    }
}