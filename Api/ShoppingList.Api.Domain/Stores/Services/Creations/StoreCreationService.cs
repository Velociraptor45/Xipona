using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public class StoreCreationService : IStoreCreationService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IStoreFactory _storeFactory;
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly CancellationToken _cancellationToken;

    public StoreCreationService(
        IStoreRepository storeRepository,
        IStoreFactory storeFactory,
        IShoppingListFactory shoppingListFactory,
        IShoppingListRepository shoppingListRepository,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepository;
        _storeFactory = storeFactory;
        _shoppingListFactory = shoppingListFactory;
        _shoppingListRepository = shoppingListRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task<IStore> CreateAsync(StoreCreation creation)
    {
        ArgumentNullException.ThrowIfNull(creation);

        _cancellationToken.ThrowIfCancellationRequested();

        IStore store = _storeFactory.CreateNew(creation);

        store = await _storeRepository.StoreAsync(store, _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        var shoppingList = _shoppingListFactory.CreateNew(store);
        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);

        return store;
    }
}