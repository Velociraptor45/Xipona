﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public interface IItem
{
    ItemId Id { get; }
    ItemName Name { get; }
    bool IsDeleted { get; }
    Comment Comment { get; }
    bool IsTemporary { get; }
    public ItemQuantity ItemQuantity { get; }
    ItemCategoryId? ItemCategoryId { get; }
    ManufacturerId? ManufacturerId { get; }
    IItem? Predecessor { get; }
    TemporaryItemId? TemporaryId { get; }
    IReadOnlyCollection<IItemAvailability> Availabilities { get; }
    IReadOnlyCollection<IItemType> ItemTypes { get; }
    bool HasItemTypes { get; }

    void Delete();

    SectionId GetDefaultSectionIdForStore(StoreId storeId);

    bool IsAvailableInStore(StoreId storeId);

    void MakePermanent(PermanentItem permanentItem, IEnumerable<IItemAvailability> availabilities);

    void Modify(ItemModification itemChange, IEnumerable<IItemAvailability> availabilities);

    Task ModifyAsync(ItemWithTypesModification modification, IValidator validator);

    void SetPredecessor(IItem predecessor);

    bool TryGetType(ItemTypeId itemTypeId, out IItemType? itemType);

    IReadOnlyCollection<IItemType> GetTypesFor(StoreId storeId);

    bool TryGetTypeWithPredecessor(ItemTypeId predecessorTypeId, out IItemType? predecessor);

    void RemoveManufacturer();
}