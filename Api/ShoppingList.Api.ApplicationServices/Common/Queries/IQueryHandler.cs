namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;

public interface IQueryHandler<in TQuery, TValue>
    where TQuery : IQuery<TValue>
{
    Task<TValue> HandleAsync(TQuery query, CancellationToken cancellationToken);
}