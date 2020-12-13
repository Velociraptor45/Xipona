using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommandHandler
        : ICommandHandler<ChangeItemQuantityOnShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;

        public ChangeItemQuantityOnShoppingListCommandHandler(IShoppingListRepository shoppingListRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<bool> HandleAsync(ChangeItemQuantityOnShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            list.ChangeItemQuantity(command.ItemId, command.Quantity);

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}