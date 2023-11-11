using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection;

public class CommandQueueConfig
{
    public TimeSpan ConnectionRetryInterval { get; init; }
}