using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.UpdateStore
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
            store.ChangeName(command.StoreUpdate.Name);

            cancellationToken.ThrowIfCancellationRequested();

            await storeRepository.StoreAsync(store, cancellationToken);

            return true;
        }
    }
}