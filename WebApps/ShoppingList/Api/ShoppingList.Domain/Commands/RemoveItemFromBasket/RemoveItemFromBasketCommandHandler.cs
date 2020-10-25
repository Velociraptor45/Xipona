using ShoppingList.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Commands.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandler : ICommandHandler<RemoveItemFromBasketCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public RemoveItemFromBasketCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(RemoveItemFromBasketCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            list.RemoveFromBasket(command.ItemId);

            await shoppingListRepository.StoreAsync(list, cancellationToken);
            return true;
        }
    }
}