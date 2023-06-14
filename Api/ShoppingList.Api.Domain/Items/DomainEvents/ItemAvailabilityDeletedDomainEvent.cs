using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;

public sealed record ItemAvailabilityDeletedDomainEvent(IItemAvailability Availability) : ItemDomainEvent;