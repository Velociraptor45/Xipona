using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromBasket
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