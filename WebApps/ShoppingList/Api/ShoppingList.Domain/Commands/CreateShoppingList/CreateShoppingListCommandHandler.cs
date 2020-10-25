using ShoppingList.Domain.Exceptions;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.CreateShoppingList
{
    public class CreateShoppingListCommandHandler : ICommandHandler<CreateShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IStoreRepository storeRepository;

        public CreateShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(CreateShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (await shoppingListRepository.ActiveShoppingListExistsForAsync(command.StoreId, cancellationToken))
            {
                throw new ShoppingListAlreadyExistsException(command.StoreId);
            }

            var store = await storeRepository.FindByAsync(command.StoreId, cancellationToken);
            var list = new Models.ShoppingList(
                new ShoppingListId(0), store, Enumerable.Empty<ShoppingListItem>(), null);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}