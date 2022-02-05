using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common;

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
            .First(interf => interf.IsGenericType && interf.GetGenericTypeDefinition() == typeof(IQuery<>))
            .GetGenericArguments()
            .First();

        var serviceType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), valueType);
        var service = _serviceProvider.GetService(serviceType);
        var method = serviceType.GetMethod("HandleAsync");

        if (method is null)
            throw new InvalidOperationException("Method 'HandleAsync' not found.");

        if (!(method.Invoke(service, new object[] { query, cancellationToken }) is Task<T> task))
        {
            throw new InvalidOperationException();
        }

        return task;
    }
}