using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure
{
    public interface ICommandDispatcher
    {
        Task<T> DispatchAsync<T>(ICommand<T> command, CancellationToken cancellationToken);
    }
}