using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.RemoveItemFromShoppingList
{
    public class RemoveItemFromShoppingListCommandHandler :
        ICommandHandler<RemoveItemFromShoppingListCommand, bool>
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ITransactionGenerator _transactionGenerator;

        public RemoveItemFromShoppingListCommandHandler(IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository, ITransactionGenerator transactionGenerator)
        {
            _shoppingListRepository = shoppingListRepository;
            _itemRepository = itemRepository;
            _transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(RemoveItemFromShoppingListCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var list = await _shoppingListRepository.FindByAsync(command.ShoppingListId, cancellationToken);
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(command.ShoppingListId));

            cancellationToken.ThrowIfCancellationRequested();

            IStoreItem? item;
            if (command.OfflineTolerantItemId.IsActualId)
            {
                ItemId itemId = new ItemId(command.OfflineTolerantItemId.ActualId!.Value);

                item = await _itemRepository.FindByAsync(itemId, cancellationToken);
                if (item == null)
                    throw new DomainException(new ItemNotFoundReason(itemId));
            }
            else
            {
                if (command.ItemTypeId != null)
                    throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

                TemporaryItemId itemId = new TemporaryItemId(command.OfflineTolerantItemId.OfflineId!.Value);

                item = await _itemRepository.FindByAsync(itemId, cancellationToken);
                if (item == null)
                    throw new DomainException(new ItemNotFoundReason(itemId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

            list.RemoveItem(item.Id, command.ItemTypeId);
            if (item.IsTemporary)
            {
                item.Delete();
                await _itemRepository.StoreAsync(item, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _shoppingListRepository.StoreAsync(list, cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}