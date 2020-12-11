using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ChangeItemCommandHandler : ICommandHandler<ChangeItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;

        public ChangeItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(ChangeItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var storeItem = await itemRepository.FindByAsync(command.ItemChange.Id, cancellationToken);

            if (storeItem == null)
                throw new ItemNotFoundException(command.ItemChange.Id);
            if (storeItem.IsTemporary)
                throw new TemporaryItemNotModifyableException(command.ItemChange.Id);

            cancellationToken.ThrowIfCancellationRequested();

            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemChange.ItemCategoryId, cancellationToken);

            Manufacturer manufacturer = null;
            if (command.ItemChange.ManufacturerId != null)
                manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemChange.ManufacturerId, cancellationToken);

            storeItem.Modify(command.ItemChange, itemCategory, manufacturer);
            var availableAtStores = storeItem.Availabilities.Select(av => av.StoreId);

            var shoppingListsWithItem = (await shoppingListRepository.FindByAsync(storeItem.Id, cancellationToken))
                .Where(list => !availableAtStores.Contains(list.Store.Id))
                .ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            foreach (var list in shoppingListsWithItem)
            {
                // remove items from all shopping lists where item is not available anymore
                list.RemoveItem(storeItem.Id.ToShoppingListItemId());
                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}