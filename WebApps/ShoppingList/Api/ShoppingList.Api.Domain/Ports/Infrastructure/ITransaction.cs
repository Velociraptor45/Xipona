using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports.Infrastructure
{
    public interface ITransaction : IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken);

        Task RollbackAsync(CancellationToken cancellationToken);
    }
}