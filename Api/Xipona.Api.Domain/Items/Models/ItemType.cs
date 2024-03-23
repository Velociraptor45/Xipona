using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public class ItemType : IItemType
{
    public ItemType(ItemTypeId id, ItemTypeName name, IEnumerable<ItemAvailability> availabilities,
        ItemTypeId? predecessorId, bool isDeleted, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        PredecessorId = predecessorId;
        Availabilities = availabilities.ToList();
        IsDeleted = isDeleted;
        CreatedAt = createdAt;

        if (Availabilities.Count == 0)
            throw new DomainException(new CannotCreateItemTypeWithoutAvailabilitiesReason());
    }

    public ItemTypeId Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<ItemAvailability> Availabilities { get; }
    public ItemTypeId? PredecessorId { get; }
    public bool IsDeleted { get; }
    public DateTimeOffset CreatedAt { get; }

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
        if (modification.Availabilities.Count == 0)
            throw new DomainException(new CannotModifyItemTypeWithoutAvailabilitiesReason());

        var domainEvents = new List<IDomainEvent>();

        await validator.ValidateAsync(modification.Availabilities);

        var newType = new ItemType(
            Id,
            modification.Name,
            modification.Availabilities,
            PredecessorId,
            IsDeleted,
            CreatedAt);

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
        if (update.Availabilities.Count == 0)
            throw new DomainException(new CannotUpdateItemTypeWithoutAvailabilitiesReason());

        await validator.ValidateAsync(update.Availabilities);

        var type = new ItemType(
            ItemTypeId.New,
            update.Name,
            update.Availabilities,
            Id,
            IsDeleted,
            CreatedAt);

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

        var newItemType = new ItemType(ItemTypeId.New, Name, availabilities, Id, IsDeleted, CreatedAt);
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
            IsDeleted,
            CreatedAt);

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
            IsDeleted,
            CreatedAt);
    }

    public IItemType Delete(out IDomainEvent? domainEventToPublish)
    {
        if (IsDeleted)
        {
            domainEventToPublish = null;
            return this;
        }

        domainEventToPublish = new ItemTypeDeletedDomainEvent(Id);
        return new ItemType(
            Id,
            Name,
            Availabilities,
            PredecessorId,
            true,
            CreatedAt);
    }

    public IItemType RemoveAvailabilitiesFor(StoreId storeId, out IEnumerable<IDomainEvent> domainEventsToPublish)
    {
        var availabilities = Availabilities.Where(av => av.StoreId != storeId).ToList();
        if (availabilities.Count == Availabilities.Count)
        {
            domainEventsToPublish = Enumerable.Empty<IDomainEvent>();
            return this;
        }

        if (availabilities.Count != 0)
        {
            var availabilitiesToRemove = Availabilities.Where(av => av.StoreId == storeId);
            domainEventsToPublish = availabilitiesToRemove.Select(av => new ItemTypeAvailabilityDeletedDomainEvent(Id, av));
            return new ItemType(
                Id,
                Name,
                availabilities,
                PredecessorId,
                IsDeleted,
                CreatedAt);
        }

        var deletedType = Delete(out var deletedDomainEvent);

        domainEventsToPublish = deletedDomainEvent != null
            ? new List<IDomainEvent> { deletedDomainEvent }
            : Enumerable.Empty<IDomainEvent>();

        return deletedType;
    }
}