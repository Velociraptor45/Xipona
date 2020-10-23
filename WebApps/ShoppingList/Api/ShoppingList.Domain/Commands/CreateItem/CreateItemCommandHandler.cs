using ShoppingList.Domain.Ports;
using System;
using System.Linq;
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

            if (!command.StoreItems.Any())
                return true;

            var id = await itemRepository.StoreAsync(command.StoreItems.First());

            foreach (var storeItem in command.StoreItems.Skip(1))
            {
                storeItem.ChangeId(id);
                await itemRepository.StoreAsync(storeItem);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }
    }
}