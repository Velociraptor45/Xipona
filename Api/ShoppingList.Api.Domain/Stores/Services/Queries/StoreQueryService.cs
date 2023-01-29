using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

public class StoreQueryService : IStoreQueryService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly CancellationToken _cancellationToken;

    public StoreQueryService(
        IStoreRepository storeRepository,
        IItemRepository itemRepository,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepository;
        _itemRepository = itemRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task<IStore> GetActiveAsync(StoreId storeId)
    {
        var store = await _storeRepository.FindActiveByAsync(storeId, _cancellationToken);

        if (store is null)
            throw new DomainException(new StoreNotFoundReason(storeId));

        return store;
    }

    public async Task<IEnumerable<IStore>> GetActiveAsync()
    {
        var activeStores = (await _storeRepository.GetActiveAsync(_cancellationToken)).ToList();
        var itemCountPerStoreDict = new Dictionary<StoreId, int>();

        foreach (var store in activeStores)
        {
            var storeId = new StoreId(store.Id);
            var items = (await _itemRepository.FindActiveByAsync(storeId, _cancellationToken)).ToList();

            itemCountPerStoreDict.Add(storeId, items.Count);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        return activeStores;
    }
}