using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore;

public class UpdateStoreCommandHandler : ICommandHandler<UpdateStoreCommand, bool>
{
    private readonly IStoreRepository _storeRepository;

    public UpdateStoreCommandHandler(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<bool> HandleAsync(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var store = await _storeRepository.FindByAsync(command.StoreUpdate.Id, cancellationToken);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(command.StoreUpdate.Id));

        store.ChangeName(command.StoreUpdate.Name);
        store.UpdateStores(command.StoreUpdate.Sections);

        cancellationToken.ThrowIfCancellationRequested();

        await _storeRepository.StoreAsync(store, cancellationToken);

        return true;
    }
}