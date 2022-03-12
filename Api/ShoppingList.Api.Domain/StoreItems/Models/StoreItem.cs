using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModifications;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public class StoreItem : IStoreItem
{
    private List<IStoreItemAvailability> _availabilities;
    private readonly ItemTypes? _itemTypes;

    public StoreItem(ItemId id, string name, bool isDeleted, string comment, bool isTemporary,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = isTemporary;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TemporaryId = temporaryId;
        _itemTypes = null;
        _availabilities = availabilities.ToList() ?? throw new ArgumentNullException(nameof(availabilities));

        // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
        // https://github.com/AutoFixture/AutoFixture/issues/1108
        Predecessor = null;
    }

    public StoreItem(ItemId id, string name, bool isDeleted, string comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        ItemTypes itemTypes)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = false;
        QuantityType = quantityType;
        QuantityInPacket = quantityInPacket;
        QuantityTypeInPacket = quantityTypeInPacket;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TemporaryId = null;
        _itemTypes = itemTypes ?? throw new ArgumentNullException(nameof(itemTypes));
        _availabilities = new List<IStoreItemAvailability>();

        if (!_itemTypes.Any())
            throw new DomainException(new CannotCreateItemWithTypesWithoutTypesReason(Id));

        // predecessor must be explicitly set via SetPredecessor(...) due to this AutoFixture bug:
        // https://github.com/AutoFixture/AutoFixture/issues/1108
        Predecessor = null;
    }

    public ItemId Id { get; }
    public string Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public string Comment { get; private set; }
    public bool IsTemporary { get; private set; }
    public QuantityType QuantityType { get; private set; }
    public float QuantityInPacket { get; private set; }
    public QuantityTypeInPacket QuantityTypeInPacket { get; private set; }
    public ItemCategoryId? ItemCategoryId { get; private set; }
    public ManufacturerId? ManufacturerId { get; private set; }
    public TemporaryItemId? TemporaryId { get; }
    public IStoreItem? Predecessor { get; private set; } // todo: change this to an IItemPredecessor model to satisfy DDD

    public IReadOnlyCollection<IItemType> ItemTypes =>
        _itemTypes?.ToList().AsReadOnly() ?? new List<IItemType>().AsReadOnly();

    public IReadOnlyCollection<IStoreItemAvailability> Availabilities => _availabilities.AsReadOnly();
    public bool HasItemTypes => _itemTypes?.Any() ?? false;

    public void Delete()
    {
        IsDeleted = true;
    }

    public bool IsAvailableInStore(StoreId storeId)
    {
        return Availabilities.Any(av => av.StoreId == storeId);
    }

    public void MakePermanent(PermanentItem permanentItem, IEnumerable<IStoreItemAvailability> availabilities)
    {
        Name = permanentItem.Name;
        Comment = permanentItem.Comment;
        QuantityType = permanentItem.QuantityType;
        QuantityInPacket = permanentItem.QuantityInPacket;
        QuantityTypeInPacket = permanentItem.QuantityTypeInPacket;
        ItemCategoryId = permanentItem.ItemCategoryId;
        ManufacturerId = permanentItem.ManufacturerId;
        _availabilities = availabilities.ToList();
        IsTemporary = false;
    }

    public void Modify(ItemModification itemChange, IEnumerable<IStoreItemAvailability> availabilities)
    {
        Name = itemChange.Name;
        Comment = itemChange.Comment;
        QuantityType = itemChange.QuantityType;
        QuantityInPacket = itemChange.QuantityInPacket;
        QuantityTypeInPacket = itemChange.QuantityTypeInPacket;
        ItemCategoryId = itemChange.ItemCategoryId;
        ManufacturerId = itemChange.ManufacturerId;
        _availabilities = availabilities.ToList();
    }

    public async Task ModifyAsync(ItemWithTypesModification modification, IValidator validator)
    {
        if (modification is null)
            throw new ArgumentNullException(nameof(modification));

        if (!HasItemTypes)
            throw new DomainException(new CannotModifyItemAsItemWithTypesReason(Id));

        if (!modification.ItemTypes.Any())
            throw new DomainException(new CannotRemoveAllTypesFromItemWithTypesReason(Id));

        Name = modification.Name;
        Comment = modification.Comment;
        QuantityType = modification.QuantityType;
        QuantityInPacket = modification.QuantityInPacket;
        QuantityTypeInPacket = modification.QuantityTypeInPacket;
        ItemCategoryId = modification.ItemCategoryId;
        ManufacturerId = modification.ManufacturerId;

        await _itemTypes!.ModifyManyAsync(modification.ItemTypes, validator);
    }

    public SectionId GetDefaultSectionIdForStore(StoreId storeId)
    {
        var availability = _availabilities.FirstOrDefault(av => av.StoreId == storeId);
        if (availability == null)
            throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

        return availability.DefaultSectionId;
    }

    public void SetPredecessor(IStoreItem predecessor)
    {
        Predecessor = predecessor;
    }

    public bool TryGetType(ItemTypeId itemTypeId, out IItemType? itemType)
    {
        if (_itemTypes is not null)
            return _itemTypes.TryGetValue(itemTypeId, out itemType);

        itemType = null;
        return false;
    }

    public IReadOnlyCollection<IItemType> GetTypesFor(StoreId storeId)
    {
        return _itemTypes?.GetForStore(storeId) ?? new List<IItemType>().AsReadOnly();
    }

    public bool TryGetTypeWithPredecessor(ItemTypeId predecessorTypeId, out IItemType? predecessor)
    {
        if (HasItemTypes)
            return _itemTypes!.TryGetWithPredecessor(predecessorTypeId, out predecessor);

        predecessor = null;
        return false;
    }
}