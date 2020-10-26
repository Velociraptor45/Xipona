using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.CreateStore
{
    public class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, bool>
    {
        private readonly IStoreRepository storeRepository;

        public CreateStoreCommandHandler(IStoreRepository storeRepository)
        {
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            cancellationToken.ThrowIfCancellationRequested();

            await storeRepository.StoreAsync(command.Store, cancellationToken);

            return true;
        }
    }
}