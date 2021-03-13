using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.UpdateStore
{
    public class UpdateStoreCommandHandler : ICommandHandler<UpdateStoreCommand, bool>
    {
        private readonly IStoreRepository storeRepository;

        public UpdateStoreCommandHandler(IStoreRepository storeRepository)
        {
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(UpdateStoreCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var store = await storeRepository.FindByAsync(command.StoreUpdate.Id, cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(command.StoreUpdate.Id));

            store.ChangeName(command.StoreUpdate.Name);
            store.UpdateStores(command.StoreUpdate.Sections);

            cancellationToken.ThrowIfCancellationRequested();

            await storeRepository.StoreAsync(store, cancellationToken);

            return true;
        }
    }
}