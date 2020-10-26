using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.FinishShoppingList
{
    public class FinishShoppingListCommandHandler : ICommandHandler<FinishShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public FinishShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(FinishShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var shoppingListWithRemainingItems = shoppingList.Finish(command.CompletionDate);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
            await shoppingListRepository.StoreAsync(shoppingListWithRemainingItems, cancellationToken);

            return true;
        }
    }
}