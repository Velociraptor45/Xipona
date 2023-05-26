using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Modifications;

public class ShoppingListModificationService : IShoppingListModificationService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly CancellationToken _cancellationToken;

    public ShoppingListModificationService(
        IShoppingListRepository shoppingListRepository,
        IItemRepository itemRepository,
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        IShoppingListSectionFactory shoppingListSectionFactory,
        CancellationToken cancellationToken)
    {
        _shoppingListRepository = shoppingListRepository;
        _itemRepository = itemRepository;
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _cancellationToken = cancellationToken;
    }

    public async Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
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
            var item = await _itemRepository.FindActiveByAsync(temporaryId, _cancellationToken);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.ChangeItemQuantity(itemId, itemTypeId, quantity);

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, _cancellationToken);
    }

    public async Task RemoveItemAndItsTypesFromCurrentListAsync(ItemId itemId)
    {
        var listsWithItem = (await _shoppingListRepository.FindActiveByAsync(itemId, _cancellationToken)).ToList();

        foreach (var list in listsWithItem)
        {
            _cancellationToken.ThrowIfCancellationRequested();

            list.RemoveItemAndItsTypes(itemId);

            await _shoppingListRepository.StoreAsync(list, _cancellationToken);
        }
    }

    public async Task RemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        _cancellationToken.ThrowIfCancellationRequested();

        IItem? item;
        if (offlineTolerantItemId.IsActualId)
        {
            ItemId itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);

            item = await _itemRepository.FindActiveByAsync(itemId, _cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            TemporaryItemId itemId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);

            item = await _itemRepository.FindActiveByAsync(itemId, _cancellationToken);
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
            IItem? item = await _itemRepository.FindActiveByAsync(temporaryId, _cancellationToken);

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
            IItem? item = await _itemRepository.FindActiveByAsync(temporaryId, _cancellationToken);

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

    public async Task RemoveSectionAsync(SectionId sectionId)
    {
        var store = await _storeRepository.FindActiveByAsync(sectionId);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(sectionId));

        var shoppingList = await _shoppingListRepository.FindActiveByAsync(store.Id, _cancellationToken);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(store.Id));

        var section = shoppingList.Sections.FirstOrDefault(s => s.Id == sectionId);
        if (section is null)
            return;

        var itemIds = section.Items.Select(i => i.Id);
        var items = (await _itemRepository.FindActiveByAsync(itemIds, _cancellationToken)).ToDictionary(i => i.Id);

        foreach (var listItem in section.Items)
        {
            if (!items.TryGetValue(listItem.Id, out var item))
                throw new DomainException(new ItemNotFoundReason(listItem.Id));

            SectionId defaultSectionId = listItem.TypeId.HasValue
                ? item.GetDefaultSectionIdForStore(shoppingList.StoreId, listItem.TypeId.Value)
                : item.GetDefaultSectionIdForStore(shoppingList.StoreId);

            if (shoppingList.Sections.All(s => s.Id != defaultSectionId))
            {
                var newSection = _shoppingListSectionFactory.CreateEmpty(defaultSectionId);
                shoppingList.AddSection(newSection);
            }

            shoppingList.TransferItem(defaultSectionId, listItem.Id, listItem.TypeId);
        }

        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
    }
}