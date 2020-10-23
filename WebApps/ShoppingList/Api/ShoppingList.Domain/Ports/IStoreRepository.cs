using ShoppingList.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IStoreRepository
    {
        Task<StoreId> StoreAsync(Store store, CancellationToken cancellationToken);
    }
}