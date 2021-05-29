using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandler : ICommandHandler<PutItemInBasketCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IItemRepository itemRepository;

        public PutItemInBasketCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<bool> HandleAsync(PutItemInBasketCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (shoppingList == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            ItemId itemId;
            if (command.OfflineTolerantItemId.IsActualId)
            {
                itemId = new ItemId(command.OfflineTolerantItemId.ActualId.Value);
            }
            else
            {
                var temporaryId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId.Value);
                IStoreItem item = await itemRepository.FindByAsync(temporaryId, cancellationToken);

                if (item == null)
                    throw new DomainException(new ItemNotFoundReason(temporaryId));

                itemId = item.Id;
            }

            shoppingList.PutItemInBasket(itemId);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

            return true;
        }
    }
}