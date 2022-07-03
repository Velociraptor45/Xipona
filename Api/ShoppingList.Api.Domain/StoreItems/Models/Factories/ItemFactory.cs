using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public class ItemFactory : IItemFactory
{
    private readonly IItemTypeFactory _itemTypeFactory;

    public ItemFactory(IItemTypeFactory itemTypeFactory)
    {
        _itemTypeFactory = itemTypeFactory;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        IItem? predecessor, IEnumerable<IItemAvailability> availabilities, TemporaryItemId? temporaryId)
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
            temporaryId);

        if (predecessor != null)
            item.SetPredecessor(predecessor);

        return item;
    }

    public IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IItem? predecessor,
        IEnumerable<IItemType> itemTypes)
    {
        var item = new Item(
            id,
            name,
            isDeleted,
            comment,
            itemQuantity,
            itemCategoryId,
            manufacturerId,
            new ItemTypes(itemTypes, _itemTypeFactory));

        if (predecessor != null)
            item.SetPredecessor(predecessor);

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
            new TemporaryItemId(model.ClientSideId));
    }

    public IItem Create(ItemUpdate itemUpdate, IItem predecessor)
    {
        var model = new Item(
            ItemId.New,
            itemUpdate.Name,
            isDeleted: false,
            itemUpdate.Comment,
            isTemporary: false,
            itemUpdate.ItemQuantity,
            itemUpdate.ItemCategoryId,
            itemUpdate.ManufacturerId,
            itemUpdate.Availabilities,
            null);

        model.SetPredecessor(predecessor);
        return model;
    }

    public IItem CreateNew(ItemName name, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IItem? predecessor,
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
            new ItemTypes(itemTypes, _itemTypeFactory));

        if (predecessor != null)
            item.SetPredecessor(predecessor);

        return item;
    }
}