namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken)
    {
        var valueType = command.GetType()
            .GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommand<>))
            .GetGenericArguments()
            .First();

        var serviceType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), valueType);
        var service = _serviceProvider.GetService(serviceType);

        if (service is null)
            throw new InvalidOperationException($"No service for type {serviceType.Name} found");

        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException($"Method 'HandleAsync' not found in service {service.GetType().Name}");

        var result = method.Invoke(service, new object[] { command, cancellationToken });

        if (result is not Task<T> typedResult)
            throw new InvalidOperationException($"Return type of service is not as expected ({result?.GetType().Name})");

        return typedResult;
    }
}