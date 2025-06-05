using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Stores.DomainEvents;

public record SectionDeletedDomainEvent(SectionId SectionId) : StoreDomainEvent
{
}
