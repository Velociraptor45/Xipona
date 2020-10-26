using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (!await itemRepository.IsValidIdAsync(command.StoreItem.Id, cancellationToken))
                throw new ItemNotFoundException(command.StoreItem.Id);

            cancellationToken.ThrowIfCancellationRequested();

            await itemRepository.StoreAsync(command.StoreItem, cancellationToken);

            return true;
        }
    }
}