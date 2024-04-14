﻿using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.Core.DomainEventHandlers;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}