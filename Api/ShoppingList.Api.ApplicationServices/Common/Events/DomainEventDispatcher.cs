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

    public Task DispatchAsync(IDomainEvent domainEvent)
    {
        var serviceType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var service = _serviceProvider.GetService(serviceType);

        if (service is null)
            throw new InvalidOperationException($"No domain event handler for type {serviceType.Name} found");

        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException($"Method 'HandleAsync' not found in service {service.GetType().Name}");

        var result = method.Invoke(service, new object[] { domainEvent, _cancellationToken });

        if (result is not Task typedResult)
            throw new InvalidOperationException($"Return type of service is not as expected ({result?.GetType().Name})");

        return typedResult;
    }
}