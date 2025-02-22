using ProjectHermes.Xipona.Api.Core.Services;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Modifications;

public class ShoppingListModificationService : IShoppingListModificationService
{
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IItemFactory _itemFactory;
    private readonly IDateTimeService _dateTimeService;
    private readonly IAddItemToShoppingListService _addItemToShoppingListService;

    public ShoppingListModificationService(IAddItemToShoppingListService addItemToShoppingListService,
        IShoppingListRepository shoppingListRepository, IItemRepository itemRepository,
        IStoreRepository storeRepository, IShoppingListSectionFactory shoppingListSectionFactory,
        IItemFactory itemFactory, IDateTimeService dateTimeService)
    {
        _addItemToShoppingListService = addItemToShoppingListService;
        _shoppingListRepository = shoppingListRepository;
        _itemRepository = itemRepository;
        _storeRepository = storeRepository;
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _itemFactory = itemFactory;
        _dateTimeService = dateTimeService;
    }

    public async Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId);
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
            var item = await _itemRepository.FindActiveByAsync(temporaryId);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.ChangeItemQuantity(itemId, itemTypeId, quantity);

        await _shoppingListRepository.StoreAsync(list);
    }

    public async Task RemoveItemAndItsTypesFromCurrentListAsync(ItemId itemId)
    {
        var listsWithItem = (await _shoppingListRepository.FindActiveByAsync(itemId)).ToList();

        foreach (var list in listsWithItem)
        {
            list.RemoveItemAndItsTypes(itemId);

            await _shoppingListRepository.StoreAsync(list);
        }
    }

    public async Task<TemporaryShoppingListItemReadModel> AddTemporaryItemAsync(ShoppingListId shoppingListId,
        ItemName itemName, QuantityType quantityType, QuantityInBasket quantity, Price price, SectionId sectionId,
        TemporaryItemId temporaryItemId)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        var item = _itemFactory.CreateTemporary(itemName, quantityType, shoppingList.StoreId, price, sectionId,
            temporaryItemId);
        await _itemRepository.StoreAsync(item);

        var shoppingListItem = await _addItemToShoppingListService.AddItemAsync(shoppingList, item, sectionId, quantity);
        await _shoppingListRepository.StoreAsync(shoppingList);

        return new TemporaryShoppingListItemReadModel(shoppingListItem.Id, shoppingListItem.IsInBasket,
            shoppingListItem.Quantity);
    }

    public async Task RemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        IItem? item;
        if (offlineTolerantItemId.IsActualId)
        {
            ItemId itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);

            item = await _itemRepository.FindActiveByAsync(itemId);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));
        }
        else
        {
            if (itemTypeId != null)
                throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

            TemporaryItemId itemId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);

            item = await _itemRepository.FindActiveByAsync(itemId);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));
        }

        list.RemoveItem(item.Id, itemTypeId);
        if (item.IsTemporary)
        {
            item.Delete();
            await _itemRepository.StoreAsync(item);
        }

        await _shoppingListRepository.StoreAsync(list);
    }

    public async Task RemoveItemFromBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId);
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
            IItem? item = await _itemRepository.FindActiveByAsync(temporaryId);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        list.RemoveFromBasket(itemId, itemTypeId);

        await _shoppingListRepository.StoreAsync(list);
    }

    public async Task PutItemInBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId);
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
            IItem? item = await _itemRepository.FindActiveByAsync(temporaryId);

            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryId));

            itemId = item.Id;
        }

        shoppingList.PutItemInBasket(itemId, itemTypeId);

        await _shoppingListRepository.StoreAsync(shoppingList);
    }

    public async Task FinishAsync(ShoppingListId shoppingListId, DateTimeOffset completionDate)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        IShoppingList nextShoppingList = shoppingList.Finish(completionDate, _dateTimeService);

        await _shoppingListRepository.StoreAsync(shoppingList);
        await _shoppingListRepository.StoreAsync(nextShoppingList);
    }

    public async Task RemoveSectionAsync(SectionId sectionId)
    {
        var store = await _storeRepository.FindActiveByAsync(sectionId);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(sectionId));

        var shoppingList = await _shoppingListRepository.FindActiveByAsync(store.Id);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(store.Id));

        var section = shoppingList.Sections.FirstOrDefault(s => s.Id == sectionId);
        if (section is null)
            return;

        var itemIds = section.Items.Select(i => i.Id);
        var items = (await _itemRepository.FindActiveByAsync(itemIds)).ToDictionary(i => i.Id);

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

        await _shoppingListRepository.StoreAsync(shoppingList);
    }

    public async Task AddDiscountAsync(ShoppingListId id, Discount discount)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(id);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(id));

        shoppingList.AddDiscount(discount);

        await _shoppingListRepository.StoreAsync(shoppingList);
    }

    public async Task RemoveDiscountAsync(ShoppingListId id, ItemId itemId, ItemTypeId? itemTypeId)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(id);
        if (shoppingList == null)
            throw new DomainException(new ShoppingListNotFoundReason(id));

        shoppingList.RemoveDiscount(itemId, itemTypeId);

        await _shoppingListRepository.StoreAsync(shoppingList);
    }
}