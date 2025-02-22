using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public interface IItemQueryService
{
    Task<ItemReadModel> GetAsync(ItemId itemId);

    Task<ItemTypePricesReadModel> GetItemTypePrices(ItemId itemId, StoreId storeId);
}