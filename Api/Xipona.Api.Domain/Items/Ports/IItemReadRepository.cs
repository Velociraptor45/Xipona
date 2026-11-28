using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Items.Services.Searches;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Ports;

public interface IItemReadRepository
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> GetShoppingListResultsAsync(string searchInput,
        StoreId storeId, int? limit);
}