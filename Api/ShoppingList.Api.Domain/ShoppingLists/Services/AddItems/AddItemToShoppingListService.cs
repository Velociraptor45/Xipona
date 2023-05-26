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

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public class AddItemToShoppingListService : IAddItemToShoppingListService
{
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IStoreRepository _storeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListItemFactory _shoppingListItemFactory;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly CancellationToken _cancellationToken;

    public AddItemToShoppingListService(
        IShoppingListSectionFactory shoppingListSectionFactory,
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        IItemRepository itemRepository,
        IShoppingListItemFactory shoppingListItemFactory,
        IShoppingListRepository shoppingListRepository,
        CancellationToken cancellationToken)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _itemRepository = itemRepository;
        _shoppingListItemFactory = shoppingListItemFactory;
        _shoppingListRepository = shoppingListRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task AddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd)
    {
        var storeIds = itemsToAdd.Select(x => x.StoreId).Distinct().ToList();
        var shoppingLists = (await _shoppingListRepository.FindActiveByAsync(storeIds, _cancellationToken))
            .ToDictionary(list => list.StoreId);
        if (storeIds.Count != shoppingLists.Count)
            throw new DomainException(new StoreNotFoundReason(storeIds.Except(shoppingLists.Select(x => x.Key))));

        var itemIds = itemsToAdd.Select(x => x.ItemId).Distinct().ToList();
        var items = (await _itemRepository.FindActiveByAsync(itemIds, _cancellationToken))
            .ToDictionary(i => i.Id);

        if (itemIds.Count != items.Count)
            throw new DomainException(new ItemNotFoundReason(itemIds.Except(items.Select(x => x.Key))));

        var stores = (await _storeRepository.FindActiveByAsync(storeIds))
            .ToDictionary(s => s.Id);
        if (stores.Count != storeIds.Count)
            throw new DomainException(new StoreNotFoundReason(storeIds.Except(stores.Select(x => x.Key))));

        foreach (var itemToAdd in itemsToAdd)
        {
            var shoppingList = shoppingLists[itemToAdd.StoreId];
            var item = items[itemToAdd.ItemId];
            var store = stores[itemToAdd.StoreId];
            if (itemToAdd.ItemTypeId is null)
            {
                await AddItemAsync(shoppingList, item, store, null, itemToAdd.Quantity, false);
            }
            else
            {
                await AddItemAsync(shoppingList, item, itemToAdd.ItemTypeId.Value, store, null,
                    itemToAdd.Quantity, false);
            }

            await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
        }
    }

    public async Task AddAsync(ShoppingListId shoppingListId, OfflineTolerantItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        var item = await LoadItemAsync(itemId);
        await AddItemAsync(list, item, null, sectionId, quantity);

        _cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, _cancellationToken);
    }

    public async Task AddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity)
    {
        var shoppingList = await LoadShoppingListAsync(shoppingListId);
        var item = await LoadItemAsync(itemId);

        await AddItemAsync(shoppingList, item, itemTypeId, null, sectionId, quantity);

        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
    }

    public async Task AddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        await AddItemAsync(shoppingList, item, itemTypeId, null, sectionId, quantity);

        await _shoppingListRepository.StoreAsync(shoppingList, _cancellationToken);
    }

    public async Task AddItemAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        IItem item = await LoadItemAsync(itemId);
        await AddItemAsync(shoppingList, item, null, sectionId, quantity);
    }

    private IShoppingListItem CreateShoppingListItem(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        return _shoppingListItemFactory.Create(itemId, itemTypeId, false, quantity);
    }

    private void ValidateItemIsAvailableAtStore(IItem item, StoreId storeId,
        out IItemAvailability availability)
    {
        var av = item.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemAtStoreNotAvailableReason(item.Id, storeId));
    }

    private static void ValidateItemTypeIsAvailableAtStore(IItemType itemType, StoreId storeId,
        out IItemAvailability availability)
    {
        var av = itemType.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemTypeAtStoreNotAvailableReason(itemType.Id, storeId));
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, IStore? store, SectionId? sectionId, QuantityInBasket quantity,
        bool throwIfItemAlreadyOnShoppingList = true)
    {
        if (!item.TryGetType(itemTypeId, out var itemType))
            throw new DomainException(new ItemTypeNotPartOfItemReason(item.Id, itemTypeId));

        ValidateItemTypeIsAvailableAtStore(itemType!, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        _cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, itemTypeId, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, store, sectionId.Value, throwIfItemAlreadyOnShoppingList);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IItem item, IStore? store,
        SectionId? sectionId, QuantityInBasket quantity, bool throwIfItemAlreadyOnShoppingList = true)
    {
        if (item.HasItemTypes)
            throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(item.Id));

        ValidateItemIsAvailableAtStore(item, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        _cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, null, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, store, sectionId.Value, throwIfItemAlreadyOnShoppingList);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IShoppingListItem item, IStore? store,
        SectionId sectionId, bool throwIfItemAlreadyOnShoppingList = true)
    {
        store ??= await _storeRepository.FindActiveByAsync(shoppingList.StoreId);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        if (!store.ContainsSection(sectionId))
            throw new DomainException(new SectionInStoreNotFoundReason(sectionId, store.Id));

        _cancellationToken.ThrowIfCancellationRequested();

        if (shoppingList.Sections.All(s => s.Id != sectionId))
        {
            var section = _shoppingListSectionFactory.CreateEmpty(sectionId);
            shoppingList.AddSection(section);
        }

        shoppingList.AddItem(item, sectionId, throwIfItemAlreadyOnShoppingList);
    }

    #region data loading

    private async Task<IShoppingList> LoadShoppingListAsync(ShoppingListId shoppingListId)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        return shoppingList;
    }

    private async Task<IItem> LoadItemAsync(ItemId itemId)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(itemId, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return item;
    }

    private async Task<IItem> LoadItemAsync(OfflineTolerantItemId offlineTolerantItemId)
    {
        if (offlineTolerantItemId.IsActualId)
            return await LoadItemAsync(new ItemId(offlineTolerantItemId.ActualId!.Value));

        IItem? item = await _itemRepository.FindActiveByAsync(
            new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value), _cancellationToken);

        if (item == null)
            throw new DomainException(new ItemNotFoundReason(new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value)));

        return item;
    }

    #endregion data loading
}