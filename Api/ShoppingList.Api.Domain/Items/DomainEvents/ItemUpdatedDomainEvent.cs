using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.DomainEvents;

public record ItemUpdatedDomainEvent(IItem NewItem) : ItemDomainEvent;