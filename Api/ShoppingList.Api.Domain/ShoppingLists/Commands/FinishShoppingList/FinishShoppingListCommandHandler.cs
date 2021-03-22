using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.FinishShoppingList
{
    public class FinishShoppingListCommandHandler : ICommandHandler<FinishShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly IShoppingListFactory shoppingListFactory;

        public FinishShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, IShoppingListFactory shoppingListFactory)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
            this.shoppingListFactory = shoppingListFactory;
        }

        public async Task<bool> HandleAsync(FinishShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var shoppingList = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (shoppingList == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            cancellationToken.ThrowIfCancellationRequested();

            shoppingList.SetCompletionDate(command.CompletionDate);
            var sections = shoppingList.RemoveItemsInBasket();
            shoppingList.RemoveItemsNotInBasket();

            var nextShoppingList = shoppingListFactory.CreateNew(shoppingList.Store, sections, null);

            cancellationToken.ThrowIfCancellationRequested();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
            await shoppingListRepository.StoreAsync(nextShoppingList, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}