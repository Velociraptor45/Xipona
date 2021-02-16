using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Commands.CreateStore
{
    public class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, bool>
    {
        private readonly IStoreRepository storeRepository;
        private readonly IStoreFactory storeFactory;

        public CreateStoreCommandHandler(IStoreRepository storeRepository, IStoreFactory storeFactory)
        {
            this.storeRepository = storeRepository;
            this.storeFactory = storeFactory;
        }

        public async Task<bool> HandleAsync(CreateStoreCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            cancellationToken.ThrowIfCancellationRequested();

            var store = storeFactory.Create(command.StoreCreationInfo.Id, command.StoreCreationInfo.Name, false,
                command.StoreCreationInfo.Sections);

            await storeRepository.StoreAsync(store, cancellationToken);

            return true;
        }
    }
}