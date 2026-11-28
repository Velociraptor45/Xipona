using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public sealed record ItemTypeDeletedDomainEvent(ItemTypeId ItemTypeId) : ItemDomainEvent;