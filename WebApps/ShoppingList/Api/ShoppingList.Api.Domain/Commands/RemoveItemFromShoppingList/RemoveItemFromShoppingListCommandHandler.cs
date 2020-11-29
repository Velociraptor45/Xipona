using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandler :
        ICommandHandler<RemoveItemFromShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;

        public RemoveItemFromShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(RemoveItemFromShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            StoreItem item = await itemRepository.FindByAsync(command.ShoppingListItemId.ToStoreItemId(), cancellationToken);

            list.RemoveItem(command.ShoppingListItemId);
            if (item.IsTemporary)
            {
                item.Delete();
                await itemRepository.StoreAsync(item, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(list, cancellationToken);
            return true;
        }
    }
}