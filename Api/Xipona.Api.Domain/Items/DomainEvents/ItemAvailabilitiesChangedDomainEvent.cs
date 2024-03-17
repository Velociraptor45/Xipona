using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.DomainEvents;
public sealed record ItemAvailabilitiesChangedDomainEvent(
    ItemTypeId? ItemTypeId,
    IReadOnlyCollection<ItemAvailability> OldAvailabilities,
    IReadOnlyCollection<ItemAvailability> NewAvailabilities) : ItemDomainEvent;