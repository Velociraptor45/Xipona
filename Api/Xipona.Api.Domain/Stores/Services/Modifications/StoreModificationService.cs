using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

public class StoreModificationService : IStoreModificationService
{
    private readonly IStoreRepository _storeRepository;

    public StoreModificationService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task ModifyAsync(StoreModification update)
    {
        var store = await _storeRepository.FindActiveByAsync(update.Id);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(update.Id));

        store.ChangeName(update.Name);
        store.ModifySectionsAsync(update.Sections);

        await _storeRepository.StoreAsync(store);
    }
}