using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.PutItemInBasket
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
            ShoppingListItemId itemId = command.ItemId;

            if (!itemId.IsActualId)
            {
                itemId = (await itemRepository.FindByAsync(command.ItemId.ToStoreItemId(), cancellationToken))
                    .Id.ToShoppingListItemId();
            }

            shoppingList.PutItemInBasket(itemId);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

            return true;
        }
    }
}