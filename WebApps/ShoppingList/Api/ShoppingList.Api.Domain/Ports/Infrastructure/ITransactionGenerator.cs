using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Ports.Infrastructure
{
    public interface ITransactionGenerator
    {
        Task<ITransaction> GenerateAsync(CancellationToken cancellationToken);
    }
}