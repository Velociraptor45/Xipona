using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;

public class ShoppingListModificationService : IShoppingListModificationService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IItemRepository _itemRepository;
    private readonly CancellationToken _cancellationToken;

    public ShoppingListModificationService(
        IShoppingListRepository shoppingListRepository,
        IItemRepository itemRepository,
        CancellationToken cancellationToken)
    {
        _shoppingListRepository = shoppingListRepository;
        _itemRepository = itemRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        ArgumentNullException.ThrowIfNull(offlineTolerantItemId);

        var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        ItemId itemId;
        if (offlineTolerantItemId.IsActualId)
        {
            itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            var temporaryId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);
            var item = await _itemRepository.FindByAsync(temporaryId, _cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.ChangeItemQuantity(itemId, itemTypeId, quantity);

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, _cancellationToken);
    }

    public async Task RemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        ArgumentNullException.ThrowIfNull(offlineTolerantItemId);

        var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        _cancellationToken.ThrowIfCancellationRequested();

        IStoreItem? item;
        if (offlineTolerantItemId.IsActualId)
        {
            ItemId itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);

            item = await _itemRepository.FindByAsync(itemId, _cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            TemporaryItemId itemId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);

            item = await _itemRepository.FindByAsync(itemId, _cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));
        }

        _cancellationToken.ThrowIfCancellationRequested();

        list.RemoveItem(item.Id, itemTypeId);
        if (item.IsTemporary)
        {
            item.Delete();
            await _itemRepository.StoreAsync(item, _cancellationToken);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, _cancellationToken);
    }

    public async Task RemoveItemFromBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId)
    {
        ArgumentNullException.ThrowIfNull(offlineTolerantItemId);

        var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        ItemId itemId;
        if (offlineTolerantItemId.IsActualId)
        {
            itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            var temporaryId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);
            IStoreItem? item = await _itemRepository.FindByAsync(temporaryId, _cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.RemoveFromBasket(itemId, itemTypeId);

        await _shoppingListRepository.StoreAsync(list, _cancellationToken);
    }

    public async Task PutItemInBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId)
    {
        ArgumentNullException.ThrowIfNull(offlineTolerantItemId);

        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        ItemId itemId;
        if (offlineTolerantItemId.IsActualId)
        {
            itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            var temporaryId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);
            IStoreItem? item = await _itemRepository.FindByAsync(temporaryId, _cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        shoppingList.PutItemInBasket(itemId, itemTypeId);

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
    }

    public async Task FinishAsync(ShoppingListId shoppingListId, DateTimeOffset completionDate)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        _cancellationToken.ThrowIfCancellationRequested();

        IShoppingList nextShoppingList = shoppingList.Finish(completionDate);

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
        await _shoppingListRepository.StoreAsync(nextShoppingList, _cancellationToken);
    }
}