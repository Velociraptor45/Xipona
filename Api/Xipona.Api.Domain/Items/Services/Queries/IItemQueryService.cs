using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Queries;

public interface IItemQueryService
{
    Task<ItemReadModel> GetAsync(ItemId itemId);

    Task<ItemTypePricesReadModel> GetItemTypePrices(ItemId itemId, StoreId storeId);
}