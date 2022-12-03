using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);
}