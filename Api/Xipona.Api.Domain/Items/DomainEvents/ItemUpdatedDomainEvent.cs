using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public record ItemUpdatedDomainEvent(IItem NewItem) : ItemDomainEvent;