using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public class AddItemToShoppingListService : IAddItemToShoppingListService
{
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IStoreRepository _storeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;

    public AddItemToShoppingListService(IShoppingListSectionFactory shoppingListSectionFactory,
        IStoreRepository storeRepository, IItemRepository itemRepository, IShoppingListRepository shoppingListRepository)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _storeRepository = storeRepository;
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task AddAsync(IEnumerable<ItemToShoppingListAddition> itemsToAdd)
    {
        var itemsToAddList = itemsToAdd.ToList();

        var storeIds = itemsToAddList.Select(x => x.StoreId).Distinct().ToList();
        var shoppingLists = (await _shoppingListRepository.FindActiveByAsync(storeIds))
            .ToDictionary(list => list.StoreId);
        if (storeIds.Count != shoppingLists.Count)
            throw new DomainException(new StoreNotFoundReason(storeIds.Except(shoppingLists.Select(x => x.Key))));

        var itemIds = itemsToAddList.Select(x => x.ItemId).Distinct().ToList();
        var items = (await _itemRepository.FindActiveByAsync(itemIds))
            .ToDictionary(i => i.Id);

        if (itemIds.Count != items.Count)
            throw new DomainException(new ItemNotFoundReason(itemIds.Except(items.Select(x => x.Key))));

        var stores = (await _storeRepository.FindActiveByAsync(storeIds))
            .ToDictionary(s => s.Id);
        if (stores.Count != storeIds.Count)
            throw new DomainException(new StoreNotFoundReason(storeIds.Except(stores.Select(x => x.Key))));

        foreach (var itemToAdd in itemsToAddList)
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

            await _shoppingListRepository.StoreAsync(shoppingList);
        }
    }

    public async Task AddAsync(ShoppingListId shoppingListId, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        var item = await LoadItemAsync(itemId);
        await AddItemAsync(list, item, null, sectionId, quantity);

        await _shoppingListRepository.StoreAsync(list);
    }

    public async Task AddItemWithTypeAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity)
    {
        var shoppingList = await LoadShoppingListAsync(shoppingListId);
        var item = await LoadItemAsync(itemId);

        await AddItemAsync(shoppingList, item, itemTypeId, null, sectionId, quantity);

        await _shoppingListRepository.StoreAsync(shoppingList);
    }

    public async Task AddItemWithTypeAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        await AddItemAsync(shoppingList, item, itemTypeId, null, sectionId, quantity);

        await _shoppingListRepository.StoreAsync(shoppingList);
    }

    public async Task AddItemAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        IItem item = await LoadItemAsync(itemId);
        await AddItemAsync(shoppingList, item, sectionId, quantity);
    }

    public async Task AddItemAsync(IShoppingList shoppingList, IItem item, SectionId? sectionId,
        QuantityInBasket quantity)
    {
        await AddItemAsync(shoppingList, item, null, sectionId, quantity);
    }

    private ShoppingListItem CreateShoppingListItem(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        return new ShoppingListItem(itemId, itemTypeId, false, quantity);
    }

    private void ValidateItemIsAvailableAtStore(IItem item, StoreId storeId,
        out ItemAvailability availability)
    {
        var av = item.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemAtStoreNotAvailableReason(item.Id, storeId));
    }

    private static void ValidateItemTypeIsAvailableAtStore(IItemType itemType, StoreId storeId,
        out ItemAvailability availability)
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

        ShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, itemTypeId, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, store, sectionId.Value, throwIfItemAlreadyOnShoppingList);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, IItem item, IStore? store,
        SectionId? sectionId, QuantityInBasket quantity, bool throwIfItemAlreadyOnShoppingList = true)
    {
        if (item.HasItemTypes)
            throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(item.Id));

        ValidateItemIsAvailableAtStore(item, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        ShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, null, quantity);
        await AddItemAsync(shoppingList, shoppingListItem, store, sectionId.Value, throwIfItemAlreadyOnShoppingList);
    }

    internal async Task AddItemAsync(IShoppingList shoppingList, ShoppingListItem item, IStore? store,
        SectionId sectionId, bool throwIfItemAlreadyOnShoppingList = true)
    {
        store ??= await _storeRepository.FindActiveByAsync(shoppingList.StoreId);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        if (!store.ContainsSection(sectionId))
            throw new DomainException(new SectionInStoreNotFoundReason(sectionId, store.Id));

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
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        return shoppingList;
    }

    private async Task<IItem> LoadItemAsync(ItemId itemId)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(itemId);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return item;
    }

    #endregion data loading
}