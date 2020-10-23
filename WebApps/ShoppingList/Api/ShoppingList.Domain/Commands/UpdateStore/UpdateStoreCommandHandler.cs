using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.UpdateStore
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

            var isIdValid = await storeRepository.IsValidIdAsync(command.Store.Id, cancellationToken);

            if (!isIdValid)
                throw new StoreNotFoundException(command.Store.Id);

            cancellationToken.ThrowIfCancellationRequested();

            await storeRepository.StoreAsync(command.Store, cancellationToken);

            return true;
        }
    }
}