using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public class ItemUpdateService : IItemUpdateService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IStoreItemFactory _storeItemFactory;
    private readonly IShoppingListExchangeService _shoppingListUpdateService;
    private readonly IValidator _validator;
    private readonly CancellationToken _cancellationToken;

    public ItemUpdateService(
        IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IItemTypeFactory itemTypeFactory,
        IStoreItemFactory storeItemFactory,
        IShoppingListExchangeService shoppingListUpdateService,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _itemTypeFactory = itemTypeFactory;
        _storeItemFactory = storeItemFactory;
        _shoppingListUpdateService = shoppingListUpdateService;
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task UpdateItemWithTypesAsync(ItemWithTypesUpdate update)
    {
        if (update is null)
            throw new ArgumentNullException(nameof(update));

        var oldItem = await _itemRepository.FindByAsync(update.OldId, _cancellationToken);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(update.OldId));
        if (!oldItem.HasItemTypes)
            throw new DomainException(new CannotUpdateItemAsItemWithTypesReason(update.OldId));

        oldItem.Delete();

        await _validator.ValidateAsync(update.ItemCategoryId);

        if (update.ManufacturerId != null)
        {
            await _validator.ValidateAsync(update.ManufacturerId.Value);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        var types = new List<IItemType>();
        foreach (var typeUpdate in update.TypeUpdates)
        {
            oldItem.TryGetType(typeUpdate.OldId, out var predecessorType);
            var type = _itemTypeFactory.CreateNew(typeUpdate.Name, typeUpdate.Availabilities, predecessorType);

            await _validator.ValidateAsync(type.Availabilities);

            types.Add(type);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        // create new Item
        var updatedItem = _storeItemFactory.CreateNew(
            update.Name,
            update.Comment,
            update.ItemQuantity,
            update.ItemCategoryId,
            update.ManufacturerId,
            oldItem,
            types);

        await _itemRepository.StoreAsync(oldItem, _cancellationToken);
        updatedItem = await _itemRepository.StoreAsync(updatedItem, _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        // change existing item references on shopping lists
        await _shoppingListUpdateService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
    }

    public async Task Update(ItemUpdate update)
    {
        ArgumentNullException.ThrowIfNull(update);

        IStoreItem? oldItem = await _itemRepository.FindByAsync(update.OldId, _cancellationToken);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(update.OldId));
        if (oldItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotUpdateableReason(update.OldId));

        oldItem.Delete();

        var itemCategoryId = update.ItemCategoryId;
        var manufacturerId = update.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        var availabilities = update.Availabilities;
        await _validator.ValidateAsync(availabilities);

        await _itemRepository.StoreAsync(oldItem, _cancellationToken);

        // create new Item
        IStoreItem updatedItem = _storeItemFactory.Create(update, oldItem);
        updatedItem = await _itemRepository.StoreAsync(updatedItem, _cancellationToken);

        // change existing item references on shopping lists
        await _shoppingListUpdateService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
    }
}