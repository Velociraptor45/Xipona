using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.DomainEvents;
public sealed record ItemCategoryDeletedDomainEvent(ItemCategoryId ItemCategoryId) : IDomainEvent;