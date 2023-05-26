using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

public class StoreModificationService : IStoreModificationService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IItemModificationService _itemModificationService;
    private readonly IShoppingListModificationService _shoppingListModificationService;

    public StoreModificationService(
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        Func<CancellationToken, IItemModificationService> itemModificationServiceDelegate,
        Func<CancellationToken, IShoppingListModificationService> shoppingListModificationServiceDelegate,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _itemModificationService = itemModificationServiceDelegate(cancellationToken);
        _shoppingListModificationService = shoppingListModificationServiceDelegate(cancellationToken);
    }

    public async Task ModifyAsync(StoreModification update)
    {
        var store = await _storeRepository.FindActiveByAsync(update.Id);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(update.Id));

        store.ChangeName(update.Name);
        await store.ModifySectionsAsync(update.Sections, _itemModificationService, _shoppingListModificationService);

        await _storeRepository.StoreAsync(store);
    }
}