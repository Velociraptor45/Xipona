using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Ports.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.FinishShoppingList
{
    public class FinishShoppingListCommandHandler : ICommandHandler<FinishShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;

        public FinishShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(FinishShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var shoppingListWithRemainingItems = shoppingList.Finish(command.CompletionDate);

            cancellationToken.ThrowIfCancellationRequested();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
            await shoppingListRepository.StoreAsync(shoppingListWithRemainingItems, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}