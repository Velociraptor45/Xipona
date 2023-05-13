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

    public AddItemToShoppingListService(IShoppingListSectionFactory shoppingListSectionFactory,
        IStoreRepository storeRepository, IItemRepository itemRepository,
        IShoppingListItemFactory shoppingListItemFactory, IShoppingListRepository shoppingListRepository)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _storeRepository = storeRepository;
        _itemRepository = itemRepository;
        _shoppingListItemFactory = shoppingListItemFactory;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task AddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd, CancellationToken cancellationToken)
    {
        var storeIds = itemsToAdd.Select(x => x.StoreId).Distinct().ToList();
        var shoppingLists = (await _shoppingListRepository.FindActiveByAsync(storeIds, cancellationToken))
            .ToDictionary(list => list.StoreId);

        if (storeIds.Count != shoppingLists.Count)
            throw new DomainException(new StoreNotFoundReason(storeIds.Except(shoppingLists.Select(x => x.Key))));

        foreach (var itemToAdd in itemsToAdd)
        {
            var shoppingList = shoppingLists[itemToAdd.StoreId];
            if (itemToAdd.ItemTypeId is null)
            {
                await AddItemAsync(shoppingList, itemToAdd.ItemId, null, itemToAdd.Quantity,
                    cancellationToken);
            }
            else
            {
                var item = await LoadItemAsync(itemToAdd.ItemId, cancellationToken);
                await AddItemAsync(shoppingList, item, itemToAdd.ItemTypeId.Value, null,
                    itemToAdd.Quantity, cancellationToken);
            }

            await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
        }
    }

    public async Task AddAsync(ShoppingListId shoppingListId, OfflineTolerantItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId, cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        var item = await LoadItemAsync(itemId, cancellationToken);
        await AddItemAsync(list, item, sectionId, quantity, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, cancellationToken);
    }

    public async Task AddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        var shoppingList = await LoadShoppingListAsync(shoppingListId, cancellationToken);
        var item = await LoadItemAsync(itemId, cancellationToken);

        await AddItemAsync(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        await AddItemAsync(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        IItem item = await LoadItemAsync(itemId, cancellationToken);
        await AddItemAsync(shoppingList, item, sectionId, quantity, cancellationToken);
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
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        if (!item.TryGetType(itemTypeId, out var itemType))
            throw new DomainException(new ItemTypeNotPartOfItemReason(item.Id, itemTypeId));

        ValidateItemTypeIsAvailableAtStore(itemType!, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, itemTypeId, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IItem item,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        if (item.HasItemTypes)
            throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(item.Id));

        ValidateItemIsAvailableAtStore(item, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, null, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IShoppingListItem item,
        SectionId sectionId, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.FindActiveByAsync(shoppingList.StoreId, cancellationToken);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        if (!store.ContainsSection(sectionId))
            throw new DomainException(new SectionInStoreNotFoundReason(sectionId, store.Id));

        cancellationToken.ThrowIfCancellationRequested();

        if (shoppingList.Sections.All(s => s.Id != sectionId))
        {
            var section = _shoppingListSectionFactory.CreateEmpty(sectionId);
            shoppingList.AddSection(section);
        }

        shoppingList.AddItem(item, sectionId);
    }

    #region data loading

    private async Task<IShoppingList> LoadShoppingListAsync(ShoppingListId shoppingListId,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, cancellationToken);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        return shoppingList;
    }

    private async Task<IItem> LoadItemAsync(ItemId itemId, CancellationToken cancellationToken)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(itemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return item;
    }

    private async Task<IItem> LoadItemAsync(OfflineTolerantItemId offlineTolerantItemId, CancellationToken cancellationToken)
    {
        if (offlineTolerantItemId.IsActualId)
            return await LoadItemAsync(new ItemId(offlineTolerantItemId.ActualId!.Value), cancellationToken);

        IItem? item = await _itemRepository.FindActiveByAsync(
            new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value), cancellationToken);

        if (item == null)
            throw new DomainException(new ItemNotFoundReason(new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value)));

        return item;
    }

    #endregion data loading
}