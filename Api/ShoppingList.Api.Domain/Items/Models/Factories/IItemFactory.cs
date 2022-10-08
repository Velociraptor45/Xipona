using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public interface IItemFactory
{
    IItem Create(ItemCreation itemCreation);

    IItem Create(TemporaryItemCreation model);

    IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        IItem? predecessor, IEnumerable<IItemAvailability> availabilities, TemporaryItemId? temporaryId,
        DateTimeOffset? updatedOn);

    IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IItem? predecessor,
        IEnumerable<IItemType> itemTypes, DateTimeOffset? updatedOn);

    IItem CreateNew(ItemName name, Comment comment, ItemQuantity itemQuantity, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, IItem? predecessor, IEnumerable<IItemType> itemTypes);
}