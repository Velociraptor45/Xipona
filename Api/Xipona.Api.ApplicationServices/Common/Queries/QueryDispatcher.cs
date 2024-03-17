namespace ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken)
    {
        var valueType = query.GetType()
            .GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>))
            .GetGenericArguments()
            .First();

        var serviceType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), valueType);
        var service = _serviceProvider.GetService(serviceType);

        if (service is null)
            throw new InvalidOperationException($"No service for type {serviceType.Name} found");

        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException($"Method 'HandleAsync' not found in service type {serviceType.Name}");

        var result = method.Invoke(service, new object[] { query, cancellationToken });

        if (result is not Task<T> typedResult)
            throw new InvalidOperationException($"Return type of service is not as expected ({result?.GetType().Name})");

        return typedResult;
    }
}