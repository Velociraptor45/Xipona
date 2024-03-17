using System.Threading.Tasks;

namespace ProjectHermes.Xipona.Api.Core.DomainEventHandlers;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}