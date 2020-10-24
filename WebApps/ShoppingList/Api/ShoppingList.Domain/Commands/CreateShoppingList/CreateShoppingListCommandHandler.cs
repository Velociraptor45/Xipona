using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.CreateShoppingList
{
    public class CreateShoppingListCommandHandler : ICommandHandler<CreateShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public CreateShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(CreateShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (await shoppingListRepository.ActiveShoppingListExistsForAsync(command.StoreId, cancellationToken))
            {
                throw new ShoppingListAlreadyExistsException(command.StoreId);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.CreateNew(command.StoreId, cancellationToken);

            return true;
        }
    }
}