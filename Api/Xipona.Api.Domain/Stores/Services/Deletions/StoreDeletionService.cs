using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Deletions;

public class StoreDeletionService : IStoreDeletionService
{
    private readonly IStoreRepository _storeRepository;

    public StoreDeletionService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task DeleteAsync(StoreId storeId)
    {
        var store = await _storeRepository.FindByAsync(storeId);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(storeId));

        store.Delete();

        await _storeRepository.StoreAsync(store);
    }
}