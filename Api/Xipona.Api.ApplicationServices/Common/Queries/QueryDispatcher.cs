using Microsoft.Extensions.DependencyInjection;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken)
    {
        var queryHandler = _serviceProvider.GetRequiredService<IQueryHandler<IQuery<T>, T>>();
        return await queryHandler.HandleAsync(query, cancellationToken);
    }
}