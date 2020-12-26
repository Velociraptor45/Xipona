using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure
{
    public interface IQueryDispatcher
    {
        Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken);
    }
}