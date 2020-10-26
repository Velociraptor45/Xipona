using ShoppingList.Api.Domain.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.ApplicationServices
{
    public interface ICommandDispatcher
    {
        Task<T> DispatchAsync<T>(ICommand<T> query, CancellationToken cancellationToken);
    }
}