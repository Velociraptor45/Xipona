using ShoppingList.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Ports
{
    public interface IShoppingListRepository
    {
        Task<Models.ShoppingList> FindActiveByStoreIdAsync(StoreId storeId, CancellationToken cancellationToken);
    }
}