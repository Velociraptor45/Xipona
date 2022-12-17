using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

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

    public async Task<IEnumerable<StoreReadModel>> GetActiveAsync()
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

        return activeStores.Select(store => new StoreReadModel(store, itemCountPerStoreDict[store.Id]));
    }
}