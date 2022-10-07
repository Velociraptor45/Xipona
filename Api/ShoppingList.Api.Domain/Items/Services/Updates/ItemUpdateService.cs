using ProjectHermes.ShoppingList.Api.Core.Services;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Exchanges;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public class ItemUpdateService : IItemUpdateService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemTypeFactory _itemTypeFactory;
    private readonly IItemFactory _itemFactory;
    private readonly IShoppingListExchangeService _shoppingListExchangeService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IValidator _validator;
    private readonly CancellationToken _cancellationToken;

    public ItemUpdateService(
        IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IItemTypeFactory itemTypeFactory,
        IItemFactory itemFactory,
        IShoppingListExchangeService shoppingListExchangeService,
        IDateTimeService dateTimeService,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _itemTypeFactory = itemTypeFactory;
        _itemFactory = itemFactory;
        _shoppingListExchangeService = shoppingListExchangeService;
        _dateTimeService = dateTimeService;
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
        var updatedItem = _itemFactory.CreateNew(
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
        await _shoppingListExchangeService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
    }

    public async Task Update(ItemUpdate update)
    {
        ArgumentNullException.ThrowIfNull(update);

        IItem? oldItem = await _itemRepository.FindByAsync(update.OldId, _cancellationToken);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(update.OldId));

        var updatedItem = await oldItem.UpdateAsync(update, _validator, _dateTimeService);

        await _itemRepository.StoreAsync(oldItem, _cancellationToken);
        updatedItem = await _itemRepository.StoreAsync(updatedItem, _cancellationToken);

        // change existing item references on shopping lists
        await _shoppingListExchangeService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
    }

    public async Task UpdateAsync(ItemId itemId, ItemTypeId? itemTypeId, StoreId storeId, Price price)
    {
        IItem? oldItem = await _itemRepository.FindByAsync(itemId, _cancellationToken);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(itemId));
        if (oldItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotUpdateableReason(itemId));

        IItem updatedItem = oldItem.Update(storeId, itemTypeId, price, _dateTimeService);
        await _itemRepository.StoreAsync(oldItem, _cancellationToken);
        updatedItem = await _itemRepository.StoreAsync(updatedItem, _cancellationToken);

        // change existing item references on shopping lists
        await _shoppingListExchangeService.ExchangeItemAsync(oldItem.Id, updatedItem, _cancellationToken);
    }
}