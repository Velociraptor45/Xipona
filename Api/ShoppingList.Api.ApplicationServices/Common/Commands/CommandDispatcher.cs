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
            .First(interf => interf.IsGenericType && interf.GetGenericTypeDefinition() == typeof(ICommand<>))
            .GetGenericArguments()
            .First();

        var serviceType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), valueType);
        var service = _serviceProvider.GetService(serviceType);
        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException("Method 'HandleAsync' not found.");

        if (!(method.Invoke(service, new object[] { command, cancellationToken }) is Task<T> task))
        {
            throw new InvalidOperationException();
        }

        return task;
    }
}