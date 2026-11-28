using System.Threading.Tasks;

namespace Xipona.Api.Core.DomainEventHandlers;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}