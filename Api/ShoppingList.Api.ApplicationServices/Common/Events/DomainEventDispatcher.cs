using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Events;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationToken _cancellationToken;

    public DomainEventDispatcher(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        _serviceProvider = serviceProvider;
        _cancellationToken = cancellationToken;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        var serviceType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var services = _serviceProvider.GetServices(serviceType).ToArray();

        if (!services.Any())
            throw new InvalidOperationException($"No domain event handler for type {serviceType.Name} found");

        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException($"Method 'HandleAsync' not found in service type {serviceType.Name}");

        foreach (var service in services)
        {
            await (Task)method.Invoke(service, new object[] { domainEvent, _cancellationToken })!;
        }
    }
}