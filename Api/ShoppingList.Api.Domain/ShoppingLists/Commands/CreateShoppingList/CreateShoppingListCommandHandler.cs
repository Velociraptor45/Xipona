using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.CreateShoppingList
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