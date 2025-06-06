using System;

namespace Xipona.Frontend.Infrastructure.Connection;

public class CommandQueueConfig
{
    public TimeSpan ConnectionRetryInterval { get; init; }
}