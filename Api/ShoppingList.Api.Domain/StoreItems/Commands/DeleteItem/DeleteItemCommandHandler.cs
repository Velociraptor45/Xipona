using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.DeleteItem
{
    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly ICommandDispatcher commandDispatcher;

        public DeleteItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, ICommandDispatcher commandDispatcher)
        {
            this.itemRepository = itemRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
            this.commandDispatcher = commandDispatcher;
        }

        public async Task<bool> HandleAsync(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var item = await itemRepository.FindByAsync(command.ItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemId));

            item.Delete();
            var listsWithItem = (await shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken)).ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemRepository.StoreAsync(item, cancellationToken);

            foreach (var list in listsWithItem)
            {
                var removeCommand = new RemoveItemFromShoppingListCommand(list.Id, item.Id.ToShoppingListItemId());
                await commandDispatcher.DispatchAsync(removeCommand, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}