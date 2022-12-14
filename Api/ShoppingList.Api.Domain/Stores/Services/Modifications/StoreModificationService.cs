using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

public class StoreModificationService : IStoreModificationService
{
    private readonly IStoreRepository _storeRepository;
    private readonly CancellationToken _cancellationToken;
    private readonly IItemModificationService _itemModificationService;
    private readonly IShoppingListModificationService _shoppingListModificationService;

    public StoreModificationService(
        IStoreRepository storeRepository,
        Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate,
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepository;
        _itemModificationService = itemModificationServiceDelegate(cancellationToken);
        _shoppingListModificationService = shoppingListModificationServiceDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task ModifyAsync(StoreModification update)
    {
        var store = await _storeRepository.FindByAsync(update.Id, _cancellationToken);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(update.Id));

        store.ChangeName(update.Name);
        await store.ModifySectionsAsync(update.Sections, _itemModificationService, _shoppingListModificationService);

        _cancellationToken.ThrowIfCancellationRequested();

        await _storeRepository.StoreAsync(store, _cancellationToken);
    }
}