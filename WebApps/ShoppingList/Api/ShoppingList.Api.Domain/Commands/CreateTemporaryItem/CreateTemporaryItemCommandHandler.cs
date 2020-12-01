using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IStoreRepository storeRepository;

        public CreateTemporaryItemCommandHandler(IItemRepository itemRepository, IStoreRepository storeRepository)
        {
            this.itemRepository = itemRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var store = await storeRepository.FindByAsync(command.TemporaryItemCreation.Availability.StoreId,
                cancellationToken);
            if (store == null || store.IsDeleted)
                throw new StoreNotFoundException(command.TemporaryItemCreation.Availability.StoreId);

            var storeItem = command.TemporaryItemCreation.ToStoreItem();

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}