using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.ApplicationServices
{
    public interface IQueryDispatcher
    {
        Task<T> DispatchAsync<T>(T query, CancellationToken cancellationToken);
    }
}