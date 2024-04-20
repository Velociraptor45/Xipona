using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

public interface IStoreQueryService
{
    Task<IEnumerable<IStore>> GetActiveAsync();

    Task<IStore> GetActiveAsync(StoreId storeId);
}