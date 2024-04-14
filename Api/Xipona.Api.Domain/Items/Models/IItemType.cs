﻿using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Updates;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Models;

public interface IItemType
{
    ItemTypeId Id { get; }
    ItemTypeName Name { get; }
    IReadOnlyCollection<ItemAvailability> Availabilities { get; }
    ItemTypeId? PredecessorId { get; }
    bool IsDeleted { get; }

    DateTimeOffset CreatedAt { get; }

    SectionId GetDefaultSectionIdForStore(StoreId storeId);

    bool IsAvailableAt(StoreId storeId);

    bool IsAvailableAt(SectionId sectionId);

    Task<(IItemType ItemType, IEnumerable<IDomainEvent> DomainEvents)> ModifyAsync(ItemTypeModification modification,
        IValidator validator);

    Task<IItemType> UpdateAsync(ItemTypeUpdate update, IValidator validator);

    IItemType Update(StoreId storeId, Price price);

    IItemType Update();

    IItemType TransferToDefaultSection(SectionId oldSectionId, SectionId newSectionId);

    IItemType Delete(out IDomainEvent? domainEventToPublish);

    IItemType RemoveAvailabilitiesFor(StoreId storeId, out IEnumerable<IDomainEvent> domainEventsToPublish);
}