using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Common.Models;
using Xipona.Api.Domain.Stores.DomainEvents;
using Xipona.Api.Domain.Stores.Reasons;
using Xipona.Api.Domain.Stores.Services.Modifications;

namespace Xipona.Api.Domain.Stores.Models;

public class Store : AggregateRoot, IStore
{
    private readonly Sections _sections;

    public Store(StoreId id, StoreName name, bool isDeleted, Sections sections, DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        CreatedAt = createdAt;
        _sections = sections;
    }

    public StoreId Id { get; }
    public StoreName Name { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public IReadOnlyCollection<ISection> Sections => _sections.AsReadOnly();

    protected override IDomainEvent OnBeforeAddingDomainEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is StoreDomainEvent storeDomainEvent)
        {
            return storeDomainEvent with { StoreId = Id };
        }

        return domainEvent;
    }

    public ISection GetDefaultSection()
    {
        return _sections.GetDefaultSection();
    }

    public bool ContainsSection(SectionId sectionId)
    {
        return _sections.Contains(sectionId);
    }

    public void ChangeName(StoreName name)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedStoreReason(Id));

        Name = name;
    }

    public void ModifySectionsAsync(IEnumerable<SectionModification> sectionModifications)
    {
        if (IsDeleted)
            throw new DomainException(new CannotModifyDeletedStoreReason(Id));

        var events = _sections.ModifyManyAsync(sectionModifications);
        PublishDomainEvents(events);
    }

    public void Delete()
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        PublishDomainEvent(new StoreDeletedDomainEvent());
    }
}