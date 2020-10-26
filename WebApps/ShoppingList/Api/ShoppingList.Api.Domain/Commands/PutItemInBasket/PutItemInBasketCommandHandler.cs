using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.PutItemInBasket
{
    public class PutItemInBasketCommandHandler : ICommandHandler<PutItemInBasketCommand, bool>
    {
        public PutItemInBasketCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            ShoppingListRepository = shoppingListRepository;
        }

        public IShoppingListRepository ShoppingListRepository { get; }

        public async Task<bool> HandleAsync(PutItemInBasketCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await ShoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            shoppingList.PutItemInBasket(command.ItemId);

            cancellationToken.ThrowIfCancellationRequested();

            await ShoppingListRepository.StoreAsync(shoppingList, cancellationToken);

            return true;
        }
    }
}