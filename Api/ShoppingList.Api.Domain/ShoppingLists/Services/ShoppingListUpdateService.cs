using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services
{
    public class ShoppingListUpdateService : IShoppingListUpdateService
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IAddItemToShoppingListService _addItemToShoppingListService;

        public ShoppingListUpdateService(IShoppingListRepository shoppingListRepository,
            IAddItemToShoppingListService addItemToShoppingListService)
        {
            _shoppingListRepository = shoppingListRepository;
            _addItemToShoppingListService = addItemToShoppingListService;
        }

        public async Task ExchangeItemAsync(ItemId oldItemId, IStoreItem newItem, CancellationToken cancellationToken)
        {
            if (oldItemId is null)
                throw new ArgumentNullException(nameof(oldItemId));
            if (newItem is null)
                throw new ArgumentNullException(nameof(newItem));

            var shoppingListsWithOldItem = (await _shoppingListRepository
                .FindActiveByAsync(oldItemId, cancellationToken))
                .ToList();

            if (newItem.HasItemTypes)
                await ExchangeItemWithTypesAsync(shoppingListsWithOldItem, oldItemId, newItem, cancellationToken);
            else
                await ExchangeItemWithoutTypesAsync(shoppingListsWithOldItem, oldItemId, newItem, cancellationToken);
        }

        private async Task ExchangeItemWithoutTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
            IStoreItem newItem, CancellationToken cancellationToken)
        {
            foreach (var list in shoppingLists)
            {
                IShoppingListItem oldListItem = list.Items
                    .First(i => i.Id == oldItemId);
                list.RemoveItem(oldListItem.Id);
                if (newItem.IsAvailableInStore(list.StoreId))
                {
                    var sectionId = newItem.GetDefaultSectionIdForStore(list.StoreId);
                    await _addItemToShoppingListService.AddItemToShoppingList(list, newItem.Id, sectionId,
                        oldListItem.Quantity, cancellationToken);

                    if (oldListItem.IsInBasket)
                        list.PutItemInBasket(newItem.Id);
                }

                await _shoppingListRepository.StoreAsync(list, cancellationToken);
            }
        }

        private async Task ExchangeItemWithTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
            IStoreItem newItem, CancellationToken cancellationToken)
        {
            foreach (var list in shoppingLists)
            {
                var oldListItems = list.Items
                    .Where(i => i.Id == oldItemId);

                foreach (var oldListItem in oldListItems)
                {
                    if (oldListItem.TypeId == null)
                        throw new DomainException(new ShoppingListItemHasNoTypeReason(list.Id, oldListItem.Id));

                    list.RemoveItem(oldItemId, oldListItem.TypeId);
                    if (!newItem.ItemTypes.TryGetWithPredecessor(oldListItem.TypeId, out var itemType)
                        || !itemType!.IsAvailableAtStore(list.StoreId))
                        continue;

                    var sectionId = itemType.GetDefaultSectionIdForStore(list.StoreId);
                    await _addItemToShoppingListService.AddItemWithTypeToShoppingList(list, newItem, itemType.Id,
                        sectionId, oldListItem.Quantity, cancellationToken);

                    if (oldListItem.IsInBasket)
                        list.PutItemInBasket(newItem.Id, itemType.Id);
                }

                await _shoppingListRepository.StoreAsync(list, cancellationToken);
            }
        }
    }
}