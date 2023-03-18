using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

public class StoreQueryService : IStoreQueryService
{
    private readonly IStoreRepository _storeRepository;
    private readonly CancellationToken _cancellationToken;

    public StoreQueryService(IStoreRepository storeRepository, CancellationToken cancellationToken)
    {
        _storeRepository = storeRepository;
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
        return await _storeRepository.GetActiveAsync(_cancellationToken);
    }
}