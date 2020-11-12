using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Ports.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.DeleteItem
{
    public class DeleteItemCommandHandler : ICommandHandler<DeleteItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;

        public DeleteItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator)
        {
            this.itemRepository = itemRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(DeleteItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            var item = await itemRepository.FindByAsync(command.ItemId, cancellationToken);
            item.Delete();
            var listsWithItem = (await shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken)).ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemRepository.StoreAsync(item, cancellationToken);

            foreach (var list in listsWithItem)
            {
                list.RemoveItem(item.Id.ToShoppingListItemId());
                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}