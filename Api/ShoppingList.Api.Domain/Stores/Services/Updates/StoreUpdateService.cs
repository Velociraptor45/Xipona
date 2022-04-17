using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

public class StoreUpdateService : IStoreUpdateService
{
    private readonly IStoreRepository _storeRepository;
    private readonly CancellationToken _cancellationToken;

    public StoreUpdateService(
        IStoreRepository storeRepository,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task UpdateAsync(StoreUpdate update)
    {
        ArgumentNullException.ThrowIfNull(update);

        var store = await _storeRepository.FindByAsync(update.Id, _cancellationToken);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(update.Id));

        store.ChangeName(update.Name);
        store.UpdateSections(update.Sections);

        _cancellationToken.ThrowIfCancellationRequested();

        await _storeRepository.StoreAsync(store, _cancellationToken);
    }
}