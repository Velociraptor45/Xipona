using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Ports;

public interface IItemTypeReadRepository
{
    Task<IEnumerable<(ItemId, ItemTypeId)>> FindActiveByAsync(string name, StoreId storeId, CancellationToken cancellationToken);
}