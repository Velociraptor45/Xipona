using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

public class ItemModificationService : IItemModificationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IValidator _validator;

    public ItemModificationService(Func<CancellationToken, IItemRepository> itemRepositoryDelegate,
        Func<CancellationToken, IValidator> validatorDelegate,
        Func<CancellationToken, IShoppingListRepository> shoppingListRepositoryDelegate,
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepositoryDelegate(cancellationToken);
        _shoppingListRepository = shoppingListRepositoryDelegate(cancellationToken);
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _validator = validatorDelegate(cancellationToken);
    }

    public async Task ModifyItemWithTypesAsync(ItemWithTypesModification modification)
    {
        var item = await _itemRepository.FindActiveByAsync(modification.Id);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(modification.Id));
        if (!item.HasItemTypes)
            throw new DomainException(new CannotModifyItemAsItemWithTypesReason(item.Id));

        var itemTypesBefore = item.ItemTypes.ToDictionary(t => t.Id);

        await item.ModifyAsync(modification, _validator);

        var itemTypesAfter = item.ItemTypes;

        foreach (var type in itemTypesAfter)
        {
            if (type.IsDeleted || !itemTypesBefore.Remove(type.Id))
                continue;

            // only remove item type from shopping list if it's not available anymore in respective store
            var availableStoreIds = type.Availabilities.Select(av => av.StoreId).ToList();
            var listsToRemoveItemFrom = (await _shoppingListRepository.FindByAsync(type.Id))
                .Where(list => availableStoreIds.All(storeId => list.StoreId != storeId));
            await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
        }

        foreach (var type in itemTypesBefore.Values)
        {
            // remove all types from shopping lists that don't exist anymore
            var listsToRemoveItemFrom = await _shoppingListRepository.FindByAsync(type.Id);
            await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
        }

        await _itemRepository.StoreAsync(item);
    }

    public async Task Modify(ItemModification modification)
    {
        var item = await _itemRepository.FindActiveByAsync(modification.Id);

        if (item == null)
            throw new DomainException(new ItemNotFoundReason(modification.Id));
        if (item.IsTemporary)
            throw new DomainException(new TemporaryItemNotModifyableReason(modification.Id));

        var itemCategoryId = modification.ItemCategoryId;
        var manufacturerId = modification.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        var availabilities = modification.Availabilities;
        await _validator.ValidateAsync(availabilities);

        item.Modify(modification, availabilities);

        var availableAtStoreIds = item.Availabilities.Select(av => av.StoreId);
        var shoppingListsWithItem = (await _shoppingListRepository.FindByAsync(item.Id))
            .Where(list => availableAtStoreIds.All(storeId => storeId != list.StoreId))
            .ToList();

        await _itemRepository.StoreAsync(item);
        foreach (var list in shoppingListsWithItem)
        {
            // remove items from all shopping lists where item is not available anymore
            list.RemoveItem(item.Id);
            await _shoppingListRepository.StoreAsync(list);
        }
    }

    public async Task TransferToSectionAsync(SectionId oldSectionId, SectionId newSectionId)
    {
        var store = await _storeRepository.FindActiveByAsync(oldSectionId);
        if (store is null)
            throw new DomainException(new StoreNotFoundReason(oldSectionId));
        if (!store.ContainsSection(newSectionId))
            throw new DomainException(new OldAndNewSectionNotInSameStoreReason(oldSectionId, newSectionId));

        var items = await _itemRepository.FindActiveByAsync(oldSectionId);

        foreach (var item in items)
        {
            item.TransferToDefaultSection(oldSectionId, newSectionId);
            await _itemRepository.StoreAsync(item);
        }
    }

    public async Task RemoveAvailabilitiesForAsync(StoreId storeId)
    {
        var items = await _itemRepository.FindActiveByAsync(storeId);

        foreach (var item in items)
        {
            item.RemoveAvailabilitiesFor(storeId);
            await _itemRepository.StoreAsync(item);
        }
    }

    private async Task RemoveItemTypeFromShoppingList(IEnumerable<IShoppingList> lists, IItem item,
        IItemType itemType)
    {
        foreach (var list in lists)
        {
            list.RemoveItem(item.Id, itemType.Id);
            await _shoppingListRepository.StoreAsync(list);
        }
    }
}