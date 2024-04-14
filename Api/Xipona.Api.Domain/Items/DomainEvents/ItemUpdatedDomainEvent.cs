using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;

public record ItemUpdatedDomainEvent(IItem NewItem) : ItemDomainEvent;