using Xipona.Api.Core.DomainEventHandlers;
using Xipona.Api.Domain.ItemCategories.Models;

namespace Xipona.Api.Domain.ItemCategories.DomainEvents;
public sealed record ItemCategoryDeletedDomainEvent(ItemCategoryId ItemCategoryId) : IDomainEvent;