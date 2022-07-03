using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

public class ItemModificationService : IItemModificationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly IValidator _validator;
    private readonly CancellationToken _cancellationToken;

    public ItemModificationService(IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IShoppingListRepository shoppingListRepository,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task ModifyItemWithTypesAsync(ItemWithTypesModification modification)
    {
        var item = await _itemRepository.FindByAsync(modification.Id, _cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(modification.Id));
        if (!item.HasItemTypes)
            throw new DomainException(new CannotModifyItemAsItemWithTypesReason(item.Id));

        var itemTypesBefore = item.ItemTypes.ToDictionary(t => t.Id);

        await item.ModifyAsync(modification, _validator);

        var itemTypesAfter = item.ItemTypes;

        foreach (var type in itemTypesAfter)
        {
            if (!itemTypesBefore.Remove(type.Id))
                continue;

            // only remove item type from shopping list if it's not available anymore in respective store
            var availableStoreIds = type.Availabilities.Select(av => av.StoreId).ToList();
            var listsToRemoveItemFrom = (await _shoppingListRepository.FindByAsync(type.Id, _cancellationToken))
                .Where(list => availableStoreIds.All(storeId => list.StoreId != storeId));
            await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
        }

        foreach (var type in itemTypesBefore.Values)
        {
            // remove all types from shopping lists that don't exist anymore
            var listsToRemoveItemFrom = await _shoppingListRepository.FindByAsync(type.Id, _cancellationToken);
            await RemoveItemTypeFromShoppingList(listsToRemoveItemFrom, item, type);
        }

        await _itemRepository.StoreAsync(item, _cancellationToken);
    }

    public async Task Modify(ItemModification modification)
    {
        ArgumentNullException.ThrowIfNull(modification);

        var storeItem = await _itemRepository.FindByAsync(modification.Id, _cancellationToken);

        if (storeItem == null)
            throw new DomainException(new ItemNotFoundReason(modification.Id));
        if (storeItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotModifyableReason(modification.Id));

        var itemCategoryId = modification.ItemCategoryId;
        var manufacturerId = modification.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        _cancellationToken.ThrowIfCancellationRequested();

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        var availabilities = modification.Availabilities;
        await _validator.ValidateAsync(availabilities);

        _cancellationToken.ThrowIfCancellationRequested();

        storeItem.Modify(modification, availabilities);

        var availableAtStoreIds = storeItem.Availabilities.Select(av => av.StoreId);
        var shoppingListsWithItem = (await _shoppingListRepository.FindByAsync(storeItem.Id, _cancellationToken))
            .Where(list => availableAtStoreIds.All(storeId => storeId != list.StoreId))
            .ToList();

        await _itemRepository.StoreAsync(storeItem, _cancellationToken);
        foreach (var list in shoppingListsWithItem)
        {
            _cancellationToken.ThrowIfCancellationRequested();
            // remove items from all shopping lists where item is not available anymore
            list.RemoveItem(storeItem.Id);
            await _shoppingListRepository.StoreAsync(list, _cancellationToken);
        }
    }

    private async Task RemoveItemTypeFromShoppingList(IEnumerable<IShoppingList> lists, IItem item,
        IItemType itemType)
    {
        foreach (var list in lists)
        {
            list.RemoveItem(item.Id, itemType.Id);
            await _shoppingListRepository.StoreAsync(list, _cancellationToken);
        }
    }
}