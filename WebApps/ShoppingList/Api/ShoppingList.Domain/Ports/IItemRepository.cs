using ShoppingList.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IItemRepository
    {
        Task<StoreItem> FindByAsync(StoreItemId storeItemId, StoreId storeId, CancellationToken cancellationToken);
        Task<StoreItemId> StoreAsync(StoreItem storeItem);
    }
}