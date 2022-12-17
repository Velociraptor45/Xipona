using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

public interface IStoreRepository
{
    Task<IEnumerable<IStore>> GetActiveAsync(CancellationToken cancellationToken);

    Task<IStore?> FindByAsync(StoreId id, CancellationToken cancellationToken);
    
    Task<IStore> StoreAsync(IStore store, CancellationToken cancellationToken);

    Task<IStore?> FindActiveByAsync(StoreId id, CancellationToken cancellationToken);

    Task<IEnumerable<IStore>> FindActiveByAsync(IEnumerable<StoreId> ids, CancellationToken cancellationToken);

    Task<IStore?> FindActiveByAsync(SectionId sectionId, CancellationToken cancellationToken);
}