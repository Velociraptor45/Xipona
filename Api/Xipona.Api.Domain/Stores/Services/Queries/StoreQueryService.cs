using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Queries;

public class StoreQueryService : IStoreQueryService
{
    private readonly IStoreRepository _storeRepository;

    public StoreQueryService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<IStore> GetActiveAsync(StoreId storeId)
    {
        var store = await _storeRepository.FindActiveByAsync(storeId);

        if (store is null)
            throw new DomainException(new StoreNotFoundReason(storeId));

        return store;
    }

    public async Task<IEnumerable<IStore>> GetActiveAsync()
    {
        return await _storeRepository.GetActiveAsync();
    }
}