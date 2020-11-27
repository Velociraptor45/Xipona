using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;

        public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var storeItem = await itemRepository.FindByAsync(command.ShoppingListItemId.ToStoreItemId(),
                list.Store.Id, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            list.AddItem(storeItem, false, command.Quantity);

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}