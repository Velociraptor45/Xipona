﻿using ProjectHermes.Xipona.Api.Core.DomainEventHandlers;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.DomainEvents;

public class StoreDeletedDomainEvent : IDomainEvent
{
    public StoreDeletedDomainEvent(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}