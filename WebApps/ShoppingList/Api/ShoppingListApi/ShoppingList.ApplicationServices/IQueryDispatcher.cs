using ShoppingList.Domain.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.ApplicationServices
{
    public interface IQueryDispatcher
    {
        Task<T> DispatchAsync<T>(IQuery<T> query, CancellationToken cancellationToken);
    }
}