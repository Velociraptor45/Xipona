using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
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

            await storeRepository.StoreAsync(command.Store, (CancellationToken)cancellationToken);

            return true;
        }
    }
}