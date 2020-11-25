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

        public CreateTemporaryItemCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var storeItem = command.TemporaryItemCreation.ToStoreItem();

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}