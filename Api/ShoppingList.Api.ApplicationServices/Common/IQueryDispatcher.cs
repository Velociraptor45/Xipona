using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common;

public interface IQueryDispatcher
{
    Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken);
}