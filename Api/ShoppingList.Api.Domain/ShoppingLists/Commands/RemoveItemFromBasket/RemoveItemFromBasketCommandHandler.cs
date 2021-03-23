using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandler : ICommandHandler<RemoveItemFromBasketCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;

        public RemoveItemFromBasketCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            ItemId itemId;
            if (command.ItemId.IsActualId)
            {
                itemId = new ItemId(command.ItemId.ActualId.Value);
            }
            else
            {
                itemId = (await itemRepository.FindByAsync(new TemporaryItemId(command.ItemId.OfflineId.Value), cancellationToken))
                    .Id;
            }

            list.RemoveFromBasket(itemId);

            await shoppingListRepository.StoreAsync(list, cancellationToken);
            return true;
        }
    }
}