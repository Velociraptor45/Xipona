using System;

namespace ProjectHermes.Xipona.Frontend.Infrastructure.Connection;

public class CommandQueueConfig
{
    public TimeSpan ConnectionRetryInterval { get; init; }
}