using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public interface IStoreItem
{
    ItemId Id { get; }
    string Name { get; }
    bool IsDeleted { get; }
    string Comment { get; }
    bool IsTemporary { get; }
    QuantityType QuantityType { get; }
    float QuantityInPacket { get; }
    QuantityTypeInPacket QuantityTypeInPacket { get; }
    ItemCategoryId? ItemCategoryId { get; }
    ManufacturerId? ManufacturerId { get; }
    IStoreItem? Predecessor { get; }
    TemporaryItemId? TemporaryId { get; }
    IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
    IReadOnlyCollection<IItemType> ItemTypes { get; }
    bool HasItemTypes { get; }

    void Delete();

    SectionId GetDefaultSectionIdForStore(StoreId storeId);

    bool IsAvailableInStore(StoreId storeId);

    void MakePermanent(PermanentItem permanentItem, IEnumerable<IStoreItemAvailability> availabilities);

    void Modify(ItemModification itemChange, IEnumerable<IStoreItemAvailability> availabilities);

    Task ModifyAsync(ItemWithTypesModification modification, IValidator validator);

    void SetPredecessor(IStoreItem predecessor);

    bool TryGetType(ItemTypeId itemTypeId, out IItemType? itemType);

    IReadOnlyCollection<IItemType> GetTypesFor(StoreId storeId);

    bool TryGetTypeWithPredecessor(ItemTypeId predecessorTypeId, out IItemType? predecessor);
}