using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.DomainEvents;

public sealed record ItemMergedDomainEvent(ItemId OriginalItemId, ItemId NewItemId, ItemTypeId NewItemTypeId) : ItemDomainEvent;