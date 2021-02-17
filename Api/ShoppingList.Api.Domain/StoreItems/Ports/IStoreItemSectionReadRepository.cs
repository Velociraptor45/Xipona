using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports
{
    public interface IStoreItemSectionReadRepository
    {
        Task<IEnumerable<IStoreItemSection>> FindByAsync(IEnumerable<StoreItemSectionId> storeItemSectionIds, CancellationToken cancellationToken);
        Task<IStoreItemSection> FindByAsync(StoreItemSectionId storeItemSectionId, CancellationToken cancellationToken);
    }
}