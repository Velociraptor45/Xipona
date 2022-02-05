using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;

public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
{
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IAvailabilityValidationService _availabilityValidationService;
    private readonly IItemRepository _itemRepository;
    private readonly IStoreItemFactory _storeItemFactory;

    public CreateItemCommandHandler(IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        IAvailabilityValidationService availabilityValidationService,
        IItemRepository itemRepository, IStoreItemFactory storeItemFactory)
    {
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _availabilityValidationService = availabilityValidationService;
        _itemRepository = itemRepository;
        _storeItemFactory = storeItemFactory;
    }

    public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        var itemCategoryId = command.ItemCreation.ItemCategoryId;
        var manufacturerId = command.ItemCreation.ManufacturerId;

        await _itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

        if (manufacturerId != null)
        {
            await _manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();

        var availabilities = command.ItemCreation.Availabilities;
        await _availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

        var storeItem = _storeItemFactory.Create(command.ItemCreation);

        await _itemRepository.StoreAsync(storeItem, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return true;
    }
}