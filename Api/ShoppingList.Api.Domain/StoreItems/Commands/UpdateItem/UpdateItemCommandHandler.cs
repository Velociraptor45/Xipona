using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;

public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
{
    private readonly IItemRepository _itemRepository;
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly IStoreItemFactory _storeItemFactory;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IAvailabilityValidationService _availabilityValidationService;
    private readonly IShoppingListUpdateService _shoppingListUpdateService;

    public UpdateItemCommandHandler(IItemRepository itemRepository,
        ITransactionGenerator transactionGenerator, IStoreItemFactory storeItemFactory,
        IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        IAvailabilityValidationService availabilityValidationService,
        IShoppingListUpdateService shoppingListUpdateService)
    {
        _itemRepository = itemRepository;
        _transactionGenerator = transactionGenerator;
        _storeItemFactory = storeItemFactory;
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _availabilityValidationService = availabilityValidationService;
        _shoppingListUpdateService = shoppingListUpdateService;
    }

    public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        IStoreItem? oldItem = await _itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
        if (oldItem == null)
            throw new DomainException(new ItemNotFoundReason(command.ItemUpdate.OldId));
        if (oldItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotUpdateableReason(command.ItemUpdate.OldId));

        oldItem.Delete();

        var itemCategoryId = command.ItemUpdate.ItemCategoryId;
        var manufacturerId = command.ItemUpdate.ManufacturerId;

        await _itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

        if (manufacturerId != null)
        {
            await _manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var availabilities = command.ItemUpdate.Availabilities;
        await _availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

        using ITransaction transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        await _itemRepository.StoreAsync(oldItem, cancellationToken);

        // create new Item
        IStoreItem updatedItem = _storeItemFactory.Create(command.ItemUpdate, oldItem);
        updatedItem = await _itemRepository.StoreAsync(updatedItem, cancellationToken);

        // change existing item references on shopping lists
        await _shoppingListUpdateService.ExchangeItemAsync(oldItem.Id, updatedItem, cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}