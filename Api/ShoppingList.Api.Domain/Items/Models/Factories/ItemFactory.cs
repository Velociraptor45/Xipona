using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

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
        ItemId? predecessorId, IEnumerable<IItemAvailability> availabilities, TemporaryItemId? temporaryId,
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

    public IItem Create(TemporaryItemCreation model)
    {
        return new Item(
            ItemId.New,
            model.Name,
            false,
            Comment.Empty,
            true,
            new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(new Quantity(1), QuantityTypeInPacket.Unit)),
            null,
            null,
            model.Availability.ToMonoList(),
            new TemporaryItemId(model.ClientSideId),
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