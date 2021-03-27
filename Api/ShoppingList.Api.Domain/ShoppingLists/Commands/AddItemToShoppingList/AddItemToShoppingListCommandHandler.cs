using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.AddItemToShoppingList
{
    public class AddItemToShoppingListCommandHandler : ICommandHandler<AddItemToShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IAddItemToShoppingListService addItemToShoppingListService;

        public AddItemToShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IAddItemToShoppingListService addItemToShoppingListService)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.addItemToShoppingListService = addItemToShoppingListService;
        }

        public async Task<bool> HandleAsync(AddItemToShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            cancellationToken.ThrowIfCancellationRequested();

            if (command.ItemId.IsActualId)
            {
                var actualId = new ItemId(command.ItemId.ActualId.Value);
                await addItemToShoppingListService.AddItemToShoppingList(list, actualId, command.SectionId, command.Quantity,
                    cancellationToken);
            }
            else
            {
                var temporaryId = new TemporaryItemId(command.ItemId.OfflineId.Value);
                await addItemToShoppingListService.AddItemToShoppingList(list, temporaryId, command.SectionId,
                    command.Quantity, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await shoppingListRepository.StoreAsync(list, cancellationToken);

            return true;
        }
    }
}