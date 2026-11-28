using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public sealed record ItemAvailabilityDeletedDomainEvent(ItemAvailability Availability) : ItemDomainEvent;