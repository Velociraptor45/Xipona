using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;

public class ShoppingListExchangeService : IShoppingListExchangeService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly Func<CancellationToken, IAddItemToShoppingListService> _addItemToShoppingListServiceDelegate;
    private readonly ILogger<ShoppingListExchangeService> _logger;
    private readonly CancellationToken _cancellationToken;

    public ShoppingListExchangeService(
        Func<CancellationToken, IShoppingListRepository> shoppingListRepositoryDelegate,
        Func<CancellationToken, IAddItemToShoppingListService> addItemToShoppingListServiceDelegate,
        ILogger<ShoppingListExchangeService> logger,
        CancellationToken cancellationToken)
    {
        _shoppingListRepository = shoppingListRepositoryDelegate(cancellationToken);
        _addItemToShoppingListServiceDelegate = addItemToShoppingListServiceDelegate;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task ExchangeItemAsync(ItemId oldItemId, IItem newItem)
    {
        var shoppingListsWithOldItem = (await _shoppingListRepository
                .FindActiveByAsync(oldItemId))
            .ToList();

        if (newItem.HasItemTypes)
            await ExchangeItemWithTypesAsync(shoppingListsWithOldItem, oldItemId, newItem);
        else
            await ExchangeItemWithoutTypesAsync(shoppingListsWithOldItem, oldItemId, newItem);
    }

    private async Task ExchangeItemWithoutTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
        IItem newItem)
    {
        var addItemToShoppingListService = _addItemToShoppingListServiceDelegate(_cancellationToken);
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
                await addItemToShoppingListService.AddItemAsync(list, newItem.Id, sectionId,
                    oldListItem.Quantity);

                if (oldListItem.IsInBasket)
                    list.PutItemInBasket(newItem.Id);
            }

            await _shoppingListRepository.StoreAsync(list);
        }
    }

    private async Task ExchangeItemWithTypesAsync(IEnumerable<IShoppingList> shoppingLists, ItemId oldItemId,
        IItem newItem)
    {
        var addItemToShoppingListService = _addItemToShoppingListServiceDelegate(_cancellationToken);
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

                await addItemToShoppingListService.AddItemWithTypeAsync(list, newItem, itemType.Id,
                    sectionId, oldListItem.Quantity);

                if (oldListItem.IsInBasket)
                    list.PutItemInBasket(newItem.Id, itemType.Id);
            }

            await _shoppingListRepository.StoreAsync(list);
        }
    }
}