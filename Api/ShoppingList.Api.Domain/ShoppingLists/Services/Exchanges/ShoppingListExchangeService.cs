﻿using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

public class ShoppingListExchangeService : IShoppingListExchangeService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;
    private readonly IItemTypeReadRepository _itemTypeReadRepository;
    private readonly ILogger<ShoppingListExchangeService> _logger;

    public ShoppingListExchangeService(IShoppingListRepository shoppingListRepository,
        IAddItemToShoppingListService addItemToShoppingListService,
        IItemTypeReadRepository itemTypeReadRepository,
        ILogger<ShoppingListExchangeService> logger)
    {
        _shoppingListRepository = shoppingListRepository;
        _addItemToShoppingListService = addItemToShoppingListService;
        _itemTypeReadRepository = itemTypeReadRepository;
        _logger = logger;
    }

    public async Task ExchangeItemAsync(ItemId oldItemId, IItem newItem, CancellationToken cancellationToken)
    {
        var shoppingListsWithOldItem = (await _shoppingListRepository
                .FindActiveByAsync(oldItemId, cancellationToken))
            .ToList();

        if (newItem.HasItemTypes)
            await ExchangeItemWithTypesAsync(shoppingListsWithOldItem, oldItemId, newItem, cancellationToken);
        else
            await ExchangeItemWithoutTypesAsync(shoppingListsWithOldItem, oldItemId, newItem, cancellationToken);
    }

    private async Task ExchangeItemWithoutTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
        IItem newItem, CancellationToken cancellationToken)
    {
        foreach (var list in shoppingLists)
        {
            IShoppingListItem oldListItem = list.Items
                .First(i => i.Id == oldItemId);
            if (oldListItem.TypeId != null)
                throw new DomainException(new ShoppingListItemHasTypeReason(list.Id, oldListItem.Id));

            list.RemoveItem(oldListItem.Id);
            _logger.LogInformation(() => $"Removed item {oldListItem.Id.Value} from list {list.Id.Value}");
            if (newItem.IsAvailableInStore(list.StoreId))
            {
                var sectionId = newItem.GetDefaultSectionIdForStore(list.StoreId);
                await _addItemToShoppingListService.AddItemToShoppingListAsync(list, newItem.Id, sectionId,
                    oldListItem.Quantity, cancellationToken);

                if (oldListItem.IsInBasket)
                    list.PutItemInBasket(newItem.Id);
            }

            await _shoppingListRepository.StoreAsync(list, cancellationToken);
        }
    }

    private async Task ExchangeItemWithTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
        IItem newItem, CancellationToken cancellationToken)
    {
        foreach (var list in shoppingLists)
        {
            var oldListItems = list.Items
                .Where(i => i.Id == oldItemId)
                .ToArray();

            var oldListItemViolating = oldListItems.FirstOrDefault(i => i.TypeId is null);
            if (oldListItemViolating is not null)
                throw new DomainException(new ShoppingListItemHasNoTypeReason(list.Id, oldListItemViolating.Id));

            foreach (var oldListItem in oldListItems)
            {
                list.RemoveItem(oldItemId, oldListItem.TypeId);
                if (!newItem.TryGetTypeWithPredecessor(oldListItem.TypeId!.Value, out var itemType)
                    || !itemType!.IsAvailableAt(list.StoreId))
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