using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

public class StoreCreationService : IStoreCreationService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IStoreFactory _storeFactory;
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListRepository _shoppingListRepository;

    public StoreCreationService(IStoreRepository storeRepository, IStoreFactory storeFactory,
        IShoppingListFactory shoppingListFactory, IShoppingListRepository shoppingListRepository)
    {
        _storeRepository = storeRepository;
        _storeFactory = storeFactory;
        _shoppingListFactory = shoppingListFactory;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task<IStore> CreateAsync(StoreCreation creation)
    {
        IStore store = _storeFactory.CreateNew(creation);

        store = await _storeRepository.StoreAsync(store);

        var shoppingList = _shoppingListFactory.CreateNew(store);
        await _shoppingListRepository.StoreAsync(shoppingList);

        return store;
    }
}