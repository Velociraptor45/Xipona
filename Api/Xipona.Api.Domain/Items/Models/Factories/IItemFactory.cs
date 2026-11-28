using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Services.Creations;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Models.Factories;

public interface IItemFactory
{
    IItem Create(ItemCreation itemCreation);

    IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        ItemId? predecessorId, IEnumerable<ItemAvailability> availabilities, TemporaryItemId? temporaryId,
        DateTimeOffset? updatedOn, DateTimeOffset createdAt, bool isFavorite);

    IItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, ItemQuantity itemQuantity,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, ItemId? predecessorId,
        IEnumerable<IItemType> itemTypes, DateTimeOffset? updatedOn, DateTimeOffset createdAt, bool isFavorite);

    IItem CreateNew(ItemName name, Comment comment, ItemQuantity itemQuantity, ItemCategoryId itemCategoryId,
        ManufacturerId? manufacturerId, ItemId? predecessorId, IEnumerable<IItemType> itemTypes);

    IItem CreateTemporary(ItemName name, QuantityType quantityType, StoreId storeId, Price price,
        SectionId defaultSectionId, TemporaryItemId temporaryItemId);
}