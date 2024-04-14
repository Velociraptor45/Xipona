using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.Xipona.Api.Domain.ItemCategories.DomainEvents;
public sealed record ItemCategoryDeletedDomainEvent(ItemCategoryId ItemCategoryId) : IDomainEvent;