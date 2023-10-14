using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public class ItemType : IItemType
{
    public ItemType(ItemTypeId id, ItemTypeName name, IEnumerable<ItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted)
    {
        Id = id;
        Name = name;
        PredecessorId = predecessorId;
        Availabilities = availabilities.ToList();
        IsDeleted = isDeleted;

        if (!Availabilities.Any())
            throw new DomainException(new CannotCreateItemTypeWithoutAvailabilitiesReason());
    }

    public ItemTypeId Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<ItemAvailability> Availabilities { get; }
    public ItemTypeId? PredecessorId { get; }
    public bool IsDeleted { get; }

    public SectionId GetDefaultSectionIdForStore(StoreId storeId)
    {
        var availability = Availabilities.FirstOrDefault(av => av.StoreId == storeId);
        if (availability == null)
            throw new DomainException(new ItemTypeAtStoreNotAvailableReason(Id, storeId));

        return availability.DefaultSectionId;
    }

    public bool IsAvailableAt(StoreId storeId)
    {
        return Availabilities.Any(av => av.StoreId == storeId);
    }

    public bool IsAvailableAt(SectionId sectionId)
    {
        return Availabilities.Any(av => av.DefaultSectionId == sectionId);
    }

    public async Task<(IItemType ItemType, IEnumerable<IDomainEvent> DomainEvents)> ModifyAsync(
        ItemTypeModification modification, IValidator validator)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemTypeReason(Id));
        if (!modification.Availabilities.Any())
            throw new DomainException(new CannotModifyItemTypeWithoutAvailabilitiesReason());

        var domainEvents = new List<IDomainEvent>();

        await validator.ValidateAsync(modification.Availabilities);

        var newType = new ItemType(
            Id,
            modification.Name,
            modification.Availabilities,
            PredecessorId,
            IsDeleted);

        if (modification.Availabilities.Count != Availabilities.Count
            || !modification.Availabilities.All(av => Availabilities.Any(oldAv => oldAv == av)))
        {
            domainEvents.Add(
                new ItemAvailabilitiesChangedDomainEvent(Id, Availabilities, modification.Availabilities));
        }

        return (newType, domainEvents);
    }

    public async Task<IItemType> UpdateAsync(ItemTypeUpdate update, IValidator validator)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemTypeReason(Id));
        if (!update.Availabilities.Any())
            throw new DomainException(new CannotUpdateItemTypeWithoutAvailabilitiesReason());

        await validator.ValidateAsync(update.Availabilities);

        var type = new ItemType(
            ItemTypeId.New,
            update.Name,
            update.Availabilities,
            Id,
            IsDeleted);

        return type;
    }

    public IItemType Update(StoreId storeId, Price price)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemTypeReason(Id));

        if (Availabilities.All(av => av.StoreId != storeId))
            throw new DomainException(new ItemTypeAtStoreNotAvailableReason(Id, storeId));

        var availabilities = Availabilities.Select(av =>
            av.StoreId == storeId
                ? new ItemAvailability(storeId, price, av.DefaultSectionId)
                : av);

        var newItemType = new ItemType(ItemTypeId.New, Name, availabilities, Id, IsDeleted);
        return newItemType;
    }

    public IItemType Update()
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemTypeReason(Id));

        var newItemType = new ItemType(
            ItemTypeId.New,
            Name,
            Availabilities,
            Id,
            IsDeleted);

        return newItemType;
    }

    public IItemType TransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedItemTypeReason(Id));

        if (!IsAvailableAt(oldSectionId))
            return this;

        var availabilities = new List<ItemAvailability>();
        for (int i = 0; i < Availabilities.Count; i++)
        {
            var availability = Availabilities.ElementAt(i);
            if (availability.DefaultSectionId == oldSectionId)
                availabilities.Add(availability.TransferToDefaultSection(newSectionId));
            else
                availabilities.Add(availability);
        }

        return new ItemType(
            Id,
            Name,
            availabilities,
            PredecessorId,
            IsDeleted);
    }

    public IItemType Delete(out IDomainEvent domainEventToPublish)
    {
        domainEventToPublish = new ItemTypeDeletedDomainEvent(Id);
        return new ItemType(
            Id,
            Name,
            Availabilities,
            PredecessorId,
            true);
    }

    public IItemType RemoveAvailabilitiesFor(StoreId storeId, out IEnumerable<IDomainEvent> domainEventsToPublish)
    {
        var availabilities = Availabilities.Where(av => av.StoreId != storeId).ToList();
        if (availabilities.Count == Availabilities.Count)
        {
            domainEventsToPublish = Enumerable.Empty<IDomainEvent>();
            return this;
        }

        if (availabilities.Any())
        {
            var availabilitiesToRemove = Availabilities.Where(av => av.StoreId == storeId);
            domainEventsToPublish = availabilitiesToRemove.Select(av => new ItemTypeAvailabilityDeletedDomainEvent(Id, av));
            return new ItemType(
                Id,
                Name,
                availabilities,
                PredecessorId,
                IsDeleted);
        }

        var deletedType = Delete(out var deletedDomainEvent);
        domainEventsToPublish = new List<IDomainEvent> { deletedDomainEvent };
        return deletedType;
    }
}