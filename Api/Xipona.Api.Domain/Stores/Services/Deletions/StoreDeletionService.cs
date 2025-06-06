using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Ports;
using Xipona.Api.Domain.Stores.Reasons;

namespace Xipona.Api.Domain.Stores.Services.Deletions;

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