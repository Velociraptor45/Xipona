using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;
public sealed record ItemAvailabilitiesChangedDomainEvent(
    ItemTypeId? ItemTypeId,
    IReadOnlyCollection<ItemAvailability> OldAvailabilities,
    IReadOnlyCollection<ItemAvailability> NewAvailabilities) : ItemDomainEvent;