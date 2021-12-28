using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemModification
{
    public class ItemModificationService : IItemModificationService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IValidator _validator;
        private readonly CancellationToken _cancellationToken;

        public ItemModificationService(IItemRepository itemRepository,
            Func<CancellationToken, IValidator> validatorDelegate,
            IShoppingListRepository shoppingListRepository,
            CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _shoppingListRepository = shoppingListRepository;
            _validator = validatorDelegate(cancellationToken);
            _cancellationToken = cancellationToken;
        }

        public async Task ModifyItemWithTypesAsync(ItemWithTypesModification modification)
        {
            var item = await _itemRepository.FindByAsync(modification.Id, _cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(modification.Id));

            var itemTypesBefore = item.ItemTypes.ToDictionary(t => t.Id);

            await item.ModifyAsync(modification, _validator);

            var itemTypesAfter = item.ItemTypes;

            foreach (var type in itemTypesAfter)
            {
                if (!itemTypesBefore.Remove(type.Id))
                    continue;

                // only remove item type from shopping list if it's not available anymore in respective store
                var availableStoreIds = type.Availabilities.Select(av => av.StoreId).ToList();
                var listsToRemoveItemFrom = (await _shoppingListRepository.FindByAsync(type.Id, _cancellationToken))
                    .Where(list => availableStoreIds.All(storeId => list.StoreId != storeId));
                await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
            }

            foreach (var type in itemTypesBefore.Values)
            {
                // remove all types from shopping lists that don't exist anymore
                var listsToRemoveItemFrom = await _shoppingListRepository.FindByAsync(type.Id, _cancellationToken);
                await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
            }

            await _itemRepository.StoreAsync(item, _cancellationToken);
        }

        private async Task RemoveItemTypeFromShoppingList(IEnumerable<IShoppingList> lists, IStoreItem item,
            IItemType itemType)
        {
            foreach (var list in lists)
            {
                list.RemoveItem(item.Id, itemType.Id);
                await _shoppingListRepository.StoreAsync(list, _cancellationToken);
            }
        }
    }
}