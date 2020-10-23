using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands
{
    public interface ICommandHandler<TCommand, TValue>
        where TCommand : ICommand<TValue>
    {
        Task<TValue> HandleAsync(TCommand command, CancellationToken cancellationToken);
    }
}