using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;

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

    public async Task AddItemWithTypeToShoppingList(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, float quantity, CancellationToken cancellationToken)
    {
        var shoppingList = await LoadShoppingList(shoppingListId, cancellationToken);
        var item = await LoadItem(itemId, cancellationToken);

        await AddItemToShoppingList(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemWithTypeToShoppingList(IShoppingList shoppingList, IStoreItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, float quantity, CancellationToken cancellationToken)
    {
        if (shoppingList is null)
            throw new ArgumentNullException(nameof(shoppingList));
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        await AddItemToShoppingList(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemToShoppingList(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        float quantity, CancellationToken cancellationToken)
    {
        if (shoppingList is null)
            throw new ArgumentNullException(nameof(shoppingList));

        IStoreItem storeItem = await LoadItem(itemId, cancellationToken);
        await AddItemToShoppingList(shoppingList, storeItem, sectionId, quantity, cancellationToken);
    }

    public async Task AddItemToShoppingList(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
        SectionId? sectionId, float quantity, CancellationToken cancellationToken)
    {
        if (shoppingList is null)
            throw new ArgumentNullException(nameof(shoppingList));

        IStoreItem storeItem = await LoadItem(temporaryItemId, cancellationToken);
        await AddItemToShoppingList(shoppingList, storeItem, sectionId, quantity, cancellationToken);
    }

    private async Task<IShoppingList> LoadShoppingList(ShoppingListId shoppingListId,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, cancellationToken);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        return shoppingList;
    }

    private async Task<IStoreItem> LoadItem(ItemId itemId, CancellationToken cancellationToken)
    {
        IStoreItem? item = await _itemRepository.FindByAsync(itemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return item;
    }

    private async Task<IStoreItem> LoadItem(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
    {
        IStoreItem? item = await _itemRepository.FindByAsync(temporaryItemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(temporaryItemId));

        return item;
    }

    private IShoppingListItem CreateShoppingListItem(ItemId itemId, ItemTypeId? itemTypeId, float quantity)
    {
        return _shoppingListItemFactory.Create(itemId, itemTypeId, false, quantity);
    }

    private void ValidateItemIsAvailableAtStore(IStoreItem storeItem, StoreId storeId,
        out IStoreItemAvailability availability)
    {
        var av = storeItem.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemAtStoreNotAvailableReason(storeItem.Id, storeId));
    }

    private void ValidateItemTypeIsAvailableAtStore(IItemType itemType, StoreId storeId,
        out IStoreItemAvailability availability)
    {
        var av = itemType.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemTypeAtStoreNotAvailableReason(itemType.Id, storeId));
    }

    internal async Task AddItemToShoppingList(IShoppingList shoppingList, IStoreItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, float quantity, CancellationToken cancellationToken)
    {
        if (!item.ItemTypes.TryGetValue(itemTypeId, out var itemType))
            throw new DomainException(new ItemTypeNotPartOfItemReason(item.Id, itemTypeId));

        ValidateItemTypeIsAvailableAtStore(itemType!, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, itemTypeId, quantity);
        await AddItemToShoppingList(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemToShoppingList(IShoppingList shoppingList, IStoreItem item,
        SectionId? sectionId, float quantity, CancellationToken cancellationToken)
    {
        if (item.HasItemTypes)
            throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(item.Id));

        ValidateItemIsAvailableAtStore(item, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, null, quantity);
        await AddItemToShoppingList(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemToShoppingList(IShoppingList shoppingList, IShoppingListItem item,
        SectionId sectionId, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);
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
}