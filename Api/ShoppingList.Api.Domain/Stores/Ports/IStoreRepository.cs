using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

public interface IStoreRepository
{
    Task<IEnumerable<IStore>> GetActiveAsync();

    Task<IStore?> FindByAsync(StoreId id);

    Task<IStore> StoreAsync(IStore store);

    Task<IStore?> FindActiveByAsync(StoreId id);

    Task<IEnumerable<IStore>> FindActiveByAsync(IEnumerable<StoreId> ids);

    Task<IStore?> FindActiveByAsync(SectionId sectionId);
}