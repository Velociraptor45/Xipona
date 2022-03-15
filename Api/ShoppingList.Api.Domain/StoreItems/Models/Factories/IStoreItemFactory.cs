using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public interface IStoreItemFactory
{
    IStoreItem Create(ItemCreation itemCreation);

    IStoreItem Create(TemporaryItemCreation model);

    IStoreItem Create(ItemUpdate itemUpdate, IStoreItem predecessor);

    IStoreItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
        IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId);

    IStoreItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, QuantityType quantityType,
        float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, IStoreItem? predecessor, IEnumerable<IItemType> itemTypes);

    IStoreItem CreateNew(ItemName name, Comment comment, QuantityType quantityType, float quantityInPacket,
        QuantityTypeInPacket quantityTypeInPacket, ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        IStoreItem? predecessor, IEnumerable<IItemType> itemTypes);
}