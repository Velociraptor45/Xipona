using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;

public record SectionDeletedDomainEvent(SectionId SectionId) : StoreDomainEvent
{
}
