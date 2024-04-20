namespace ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;

public interface IQueryDispatcher
{
    Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken);
}