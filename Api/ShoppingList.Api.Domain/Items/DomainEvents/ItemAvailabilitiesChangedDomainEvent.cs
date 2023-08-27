using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;
public sealed record ItemAvailabilitiesChangedDomainEvent(
    ItemTypeId? ItemTypeId,
    IReadOnlyCollection<ItemAvailability> OldAvailabilities,
    IReadOnlyCollection<ItemAvailability> NewAvailabilities) : ItemDomainEvent;