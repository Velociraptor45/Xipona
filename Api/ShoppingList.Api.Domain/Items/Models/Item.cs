using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Services;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public class Item : AggregateRoot, IItem
{
    private List<IItemAvailability> _availabilities;
    private readonly ItemTypes? _itemTypes;

    public Item(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<IItemAvailability> availabilities, TemporaryItemId? temporaryId, DateTimeOffset? updatedOn,
        ItemId? predecessorId)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        IsTemporary = isTemporary;
        ItemQuantity = itemQuantity;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        TemporaryId = temporaryId;
        UpdatedOn = updatedOn;
        PredecessorId = predecessorId;
        _itemTypes = null;
        _availabilities = availabilities.ToList();

        if (!_availabilities.Any())
            throw new DomainException(new CannotCreateItemWithoutAvailabilitiesReason());
    }

    public Item(ItemId id, ItemName name, bool isDeleted, Comment comment,
        ItemQuantity itemQuantity, ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        ItemTypes itemTypes, DateTimeOffset? updatedOn, ItemId? predecessorId)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        Comment = comment;
        ItemQuantity = itemQuantity;
        IsTemporary = false;
        ItemCategoryId = itemCategoryId;
        ManufacturerId = manufacturerId;
        UpdatedOn = updatedOn;
        PredecessorId = predecessorId;
        TemporaryId = null;
        _itemTypes = itemTypes;
        _availabilities = new List<IItemAvailability>();

        if (!_itemTypes.Any())
            throw new DomainException(new CannotCreateItemWithTypesWithoutTypesReason(Id));
    }

    public ItemId Id { get; }
    public ItemName Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public Comment Comment { get; private set; }
    public bool IsTemporary { get; private set; }
    public ItemQuantity ItemQuantity { get; private set; }
    public ItemCategoryId? ItemCategoryId { get; private set; }
    public ManufacturerId? ManufacturerId { get; private set; }
    public TemporaryItemId? TemporaryId { get; }
    public DateTimeOffset? UpdatedOn { get; private set; }
    public ItemId? PredecessorId { get; }

    public IReadOnlyCollection<IItemType> ItemTypes =>
        _itemTypes?.ToList().AsReadOnly() ?? new List<IItemType>().AsReadOnly();

    public IReadOnlyCollection<IItemAvailability> Availabilities => _availabilities.AsReadOnly();
    public bool HasItemTypes => _itemTypes?.Any() ?? false;

    protected override IDomainEvent OnBeforeAddingDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is ItemDomainEvent itemDomainEvent)
        {
            return itemDomainEvent with { ItemId = Id };
        }

        return domainEvent;
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        PublishDomainEvent(new ItemDeletedDomainEvent());
    }

    public bool IsAvailableInStore(StoreId storeId)
    {
        return Availabilities.Any(av => av.StoreId == storeId);
    }

    public void MakePermanent(PermanentItem permanentItem, IEnumerable<IItemAvailability> availabilities)
    {
        if (IsDeleted)
            throw new DomainException(new CannotMakeDeletedItemPermanentReason(Id));

        Name = permanentItem.Name;
        Comment = permanentItem.Comment;
        ItemQuantity = permanentItem.ItemQuantity;
        ItemCategoryId = permanentItem.ItemCategoryId;
        ManufacturerId = permanentItem.ManufacturerId;
        _availabilities = availabilities.ToList();
        IsTemporary = false;
    }

    public void Modify(ItemModification itemChange, IEnumerable<IItemAvailability> availabilities)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemReason(Id));

        Name = itemChange.Name;
        Comment = itemChange.Comment;
        ItemQuantity = itemChange.ItemQuantity;
        ItemCategoryId = itemChange.ItemCategoryId;
        ManufacturerId = itemChange.ManufacturerId;
        _availabilities = availabilities.ToList();

        if (!_availabilities.Any())
            throw new DomainException(new CannotModifyItemWithoutAvailabilitiesReason());
    }

    public async Task ModifyAsync(ItemWithTypesModification modification, IValidator validator)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemReason(Id));

        if (!HasItemTypes)
            throw new DomainException(new CannotModifyItemAsItemWithTypesReason(Id));

        if (!modification.ItemTypes.Any())
            throw new DomainException(new CannotRemoveAllTypesFromItemWithTypesReason(Id));

        Name = modification.Name;
        Comment = modification.Comment;
        ItemQuantity = modification.ItemQuantity;
        ItemCategoryId = modification.ItemCategoryId;
        ManufacturerId = modification.ManufacturerId;

        await _itemTypes!.ModifyManyAsync(modification.ItemTypes, validator);
    }

    public SectionId GetDefaultSectionIdForStore(StoreId storeId)
    {
        if (HasItemTypes)
            throw new DomainException(new ItemWithTypesHasNoAvailabilitiesReason(Id));

        var availability = _availabilities.FirstOrDefault(av => av.StoreId == storeId);
        if (availability == null)
            throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

        return availability.DefaultSectionId;
    }

    public SectionId GetDefaultSectionIdForStore(StoreId storeId, ItemTypeId itemTypeId)
    {
        if (!HasItemTypes)
            throw new DomainException(new ItemHasNoItemTypesReason(Id));

        var type = _itemTypes!.FirstOrDefault(t => t.Id == itemTypeId);
        if (type is null)
            throw new DomainException(new ItemTypeNotFoundReason(Id, itemTypeId));

        var availability = type.Availabilities.FirstOrDefault(av => av.StoreId == storeId);
        if (availability == null)
            throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

        return availability.DefaultSectionId;
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

    public void RemoveManufacturer()
    {
        if (IsDeleted)
            throw new DomainException(new CannotRemoveManufacturerFromDeletedItemReason(Id));

        ManufacturerId = null;
    }

    public async Task<IItem> UpdateAsync(ItemWithTypesUpdate update, IValidator validator,
        IDateTimeService dateTimeService)
    {
        if (IsDeleted)
            throw new DomainException(new CannotUpdateDeletedItemReason(Id));

        if (!HasItemTypes)
            throw new DomainException(new CannotUpdateItemAsItemWithTypesReason(update.OldId));

        await validator.ValidateAsync(update.ItemCategoryId);

        if (update.ManufacturerId != null)
        {
            await validator.ValidateAsync(update.ManufacturerId.Value);
        }

        var types = await _itemTypes!.UpdateAsync(update.TypeUpdates, validator);

        // create new Item
        var updatedItem = new Item(
            ItemId.New,
            update.Name,
            isDeleted: false,
            update.Comment,
            update.ItemQuantity,
            update.ItemCategoryId,
            update.ManufacturerId,
            types,
            null,
            Id);

        PublishDomainEvent(new ItemUpdatedDomainEvent(updatedItem));
        Delete();
        UpdatedOn = dateTimeService.UtcNow;

        return updatedItem;
    }

    public async Task<IItem> UpdateAsync(ItemUpdate update, IValidator validator, IDateTimeService dateTimeService)
    {
        if (IsDeleted)
            throw new DomainException(new CannotUpdateDeletedItemReason(Id));

        if (IsTemporary)
            throw new DomainException(new TemporaryItemNotUpdateableReason(update.OldId));
        if (HasItemTypes)
            throw new DomainException(new CannotUpdateItemWithTypesAsItemReason(update.OldId));
        if (!update.Availabilities.Any())
            throw new DomainException(new CannotUpdateItemWithoutAvailabilitiesReason());

        var itemCategoryId = update.ItemCategoryId;
        var manufacturerId = update.ManufacturerId;

        await validator.ValidateAsync(itemCategoryId);

        if (manufacturerId != null)
        {
            await validator.ValidateAsync(manufacturerId.Value);
        }

        var availabilities = update.Availabilities;
        await validator.ValidateAsync(availabilities);

        var newItem = new Item(
            ItemId.New,
            update.Name,
            isDeleted: false,
            update.Comment,
            isTemporary: false,
            update.ItemQuantity,
            update.ItemCategoryId,
            update.ManufacturerId,
            update.Availabilities,
            null,
            null,
            Id);

        PublishDomainEvent(new ItemUpdatedDomainEvent(newItem));
        Delete();
        UpdatedOn = dateTimeService.UtcNow;

        return newItem;
    }

    public IItem Update(StoreId storeId, ItemTypeId? itemTypeId, Price price, IDateTimeService dateTimeService)
    {
        if (IsDeleted)
            throw new DomainException(new CannotUpdateDeletedItemReason(Id));

        IItem newItem;
        if (HasItemTypes)
        {
            var itemTypes = _itemTypes!.Update(storeId, itemTypeId, price);
            newItem = new Item(ItemId.New, Name, false, Comment, ItemQuantity, ItemCategoryId!.Value, ManufacturerId,
                itemTypes, null, Id);
        }
        else
        {
            if (Availabilities.All(av => av.StoreId != storeId))
                throw new DomainException(new ItemAtStoreNotAvailableReason(Id, storeId));

            var availabilities = _availabilities.Select(av =>
                av.StoreId == storeId
                    ? new ItemAvailability(storeId, price, av.DefaultSectionId)
                    : av);

            newItem = new Item(ItemId.New, Name, false, Comment, IsTemporary, ItemQuantity, ItemCategoryId,
                ManufacturerId, availabilities, TemporaryId, null, Id);
        }

        PublishDomainEvent(new ItemUpdatedDomainEvent(newItem));
        Delete();
        UpdatedOn = dateTimeService.UtcNow;

        return newItem;
    }

    public void TransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId)
    {
        if (IsDeleted)
            throw new DomainException(new CannotTransferDeletedItemReason(Id));

        if (HasItemTypes)
        {
            _itemTypes!.TransferToDefaultSection(oldSectionId, newSectionId);
            return;
        }

        for (int i = 0; i < _availabilities.Count; i++)
        {
            var availability = _availabilities[i];
            if (availability.DefaultSectionId == oldSectionId)
                _availabilities[i] = availability.TransferToDefaultSection(newSectionId);
        }
    }

    public void RemoveAvailabilitiesFor(StoreId storeId)
    {
        if (IsDeleted)
            return;

        if (HasItemTypes)
        {
            if (_itemTypes!.Count() == 1
               && _itemTypes!.First().Availabilities.Count == 1
               && _itemTypes!.First().IsAvailableAt(storeId))
            {
                Delete();
                return;
            }

            _itemTypes.RemoveAvailabilitiesFor(storeId, out var domainEventsToPublish);
            PublishDomainEvents(domainEventsToPublish);
            return;
        }

        var availabilities = _availabilities.Where(av => av.StoreId != storeId).ToList();
        if (availabilities.Count == _availabilities.Count)
            return;

        if (availabilities.Any())
        {
            var availabilitiesToRemove = _availabilities.Where(av => av.StoreId == storeId);
            _availabilities = availabilities;
            PublishDomainEvents(availabilitiesToRemove.Select(av => new ItemAvailabilityDeletedDomainEvent(av)));
        }
        else
        {
            Delete();
        }
    }
}