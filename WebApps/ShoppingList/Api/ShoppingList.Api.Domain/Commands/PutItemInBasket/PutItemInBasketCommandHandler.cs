using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandler : ICommandHandler<PutItemInBasketCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public PutItemInBasketCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(PutItemInBasketCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            shoppingList.PutItemInBasket(command.ItemId);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);

            return true;
        }
    }
}