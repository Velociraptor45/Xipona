using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;

public class ModifyItemCommandHandler : ICommandHandler<ModifyItemCommand, bool>
{
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ITransactionGenerator _transactionGenerator;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IAvailabilityValidationService _availabilityValidationService;

    public ModifyItemCommandHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator, IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        IAvailabilityValidationService availabilityValidationService)
    {
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _transactionGenerator = transactionGenerator;
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _availabilityValidationService = availabilityValidationService;
    }

    public async Task<bool> HandleAsync(ModifyItemCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var storeItem = await _itemRepository.FindByAsync(command.ItemModify.Id, cancellationToken);

        if (storeItem == null)
            throw new DomainException(new ItemNotFoundReason(command.ItemModify.Id));
        if (storeItem.IsTemporary)
            throw new DomainException(new TemporaryItemNotModifyableReason(command.ItemModify.Id));

        var itemCategoryId = command.ItemModify.ItemCategoryId;
        var manufacturerId = command.ItemModify.ManufacturerId;

        await _itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (manufacturerId != null)
        {
            await _manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var availabilities = command.ItemModify.Availabilities;
        await _availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        storeItem.Modify(command.ItemModify, availabilities);

        var availableAtStoreIds = storeItem.Availabilities.Select(av => av.StoreId);
        var shoppingListsWithItem = (await _shoppingListRepository.FindByAsync(storeItem.Id, cancellationToken))
            .Where(list => availableAtStoreIds.All(storeId => storeId != list.StoreId))
            .ToList();

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);

        await _itemRepository.StoreAsync(storeItem, cancellationToken);
        foreach (var list in shoppingListsWithItem)
        {
            cancellationToken.ThrowIfCancellationRequested();
            // remove items from all shopping lists where item is not available anymore
            list.RemoveItem(storeItem.Id);
            await _shoppingListRepository.StoreAsync(list, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return true;
    }
}