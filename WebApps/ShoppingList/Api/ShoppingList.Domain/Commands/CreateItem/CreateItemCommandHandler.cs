using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            cancellationToken.ThrowIfCancellationRequested();

            await itemRepository.StoreAsync(command.StoreItem, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }
    }
}