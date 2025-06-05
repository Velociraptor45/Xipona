using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public sealed record ItemTypeAvailabilityDeletedDomainEvent(ItemTypeId ItemTypeId, ItemAvailability Availability)
    : ItemDomainEvent;