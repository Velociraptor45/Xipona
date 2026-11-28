using System.Threading;
using System.Threading.Tasks;

namespace Xipona.Api.Core.DomainEventHandlers;

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}