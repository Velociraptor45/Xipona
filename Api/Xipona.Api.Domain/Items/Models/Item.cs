using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public class Item : AggregateRoot, IItem
{
    private List<ItemAvailability> _availabilities;
    private readonly ItemTypes? _itemTypes;

    public Item(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        ItemQuantity itemQuantity, ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId,
        IEnumerable<ItemAvailability> availabilities, TemporaryItemId? temporaryId, DateTimeOffset? updatedOn,
        ItemId? predecessorId, DateTimeOffset createdAt)
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
        CreatedAt = createdAt;
        _itemTypes = null;
        _availabilities = availabilities.ToList();

        if (_availabilities.Count == 0)
            throw new DomainException(new CannotCreateItemWithoutAvailabilitiesReason());
    }

    public Item(ItemId id, ItemName name, bool isDeleted, Comment comment,
        ItemQuantity itemQuantity, ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId,
        ItemTypes itemTypes, DateTimeOffset? updatedOn, ItemId? predecessorId, DateTimeOffset createdAt)
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
        CreatedAt = createdAt;
        TemporaryId = null;
        _itemTypes = itemTypes;
        _availabilities = new List<ItemAvailability>();

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
    public DateTimeOffset CreatedAt { get; }

    public IReadOnlyCollection<IItemType> ItemTypes =>
        _itemTypes?.ToList().AsReadOnly() ?? new List<IItemType>().AsReadOnly();

    public IReadOnlyCollection<ItemAvailability> Availabilities => _availabilities.AsReadOnly();
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

    public void MakePermanent(PermanentItem permanentItem, IEnumerable<ItemAvailability> availabilities)
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

    public void Modify(ItemModification modification, IEnumerable<ItemAvailability> availabilities)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemReason(Id));

        if (HasItemTypes)
            throw new DomainException(new CannotModifyItemWithTypesAsItemReason(Id));

        Name = modification.Name;
        Comment = modification.Comment;
        ItemQuantity = modification.ItemQuantity;
        ItemCategoryId = modification.ItemCategoryId;
        ManufacturerId = modification.ManufacturerId;

        var oldAvailabilities = _availabilities;
        _availabilities = availabilities.ToList();

        if (_availabilities.Count == 0)
            throw new DomainException(new CannotModifyItemWithoutAvailabilitiesReason());

        if (_availabilities.Count != oldAvailabilities.Count
           || !_availabilities.All(av => oldAvailabilities.Any(oldAv => oldAv == av)))
        {
            PublishDomainEvent(new ItemAvailabilitiesChangedDomainEvent(null, oldAvailabilities, _availabilities));
        }
    }

    public async Task ModifyAsync(ItemWithTypesModification modification, IValidator validator)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemReason(Id));

        if (!HasItemTypes)
            throw new DomainException(new CannotModifyItemAsItemWithTypesReason(Id));

        if (modification.ItemTypes.Count == 0)
            throw new DomainException(new CannotRemoveAllTypesFromItemWithTypesReason(Id));

        Name = modification.Name;
        Comment = modification.Comment;
        ItemQuantity = modification.ItemQuantity;
        ItemCategoryId = modification.ItemCategoryId;
        ManufacturerId = modification.ManufacturerId;

        var domainEvents = await _itemTypes!.ModifyManyAsync(modification.ItemTypes, validator);
        PublishDomainEvents(domainEvents);
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
        return _itemTypes?.GetForStore(storeId) ?? new List<IItemType>(0).AsReadOnly();
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
            Id,
            CreatedAt);

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
        if (update.Availabilities.Count == 0)
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
            Id,
            CreatedAt);

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
                itemTypes, null, Id, CreatedAt);
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
                ManufacturerId, availabilities, TemporaryId, null, Id, CreatedAt);
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
            if (_itemTypes!.All(t => t.IsAvailableAt(storeId) && t.Availabilities.Count == 1))
            {
                Delete();
                return;
            }

            _itemTypes!.RemoveAvailabilitiesFor(storeId, out var domainEventsToPublish);
            PublishDomainEvents(domainEventsToPublish);
            return;
        }

        var availabilities = _availabilities.Where(av => av.StoreId != storeId).ToList();
        if (availabilities.Count == _availabilities.Count)
            return;

        if (availabilities.Count != 0)
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