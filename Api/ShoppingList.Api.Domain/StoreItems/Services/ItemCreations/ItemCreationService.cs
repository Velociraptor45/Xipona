using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validation;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemCreations;

public class ItemCreationService : IItemCreationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;
    private readonly IStoreItemFactory _storeItemFactory;
    private readonly CancellationToken _cancellationToken;

    public ItemCreationService(
        IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IStoreItemFactory storeItemFactory,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _storeItemFactory = storeItemFactory;
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task CreateAsync(ItemCreation creation)
    {
        ArgumentNullException.ThrowIfNull(creation);

        var itemCategoryId = creation.ItemCategoryId;
        var manufacturerId = creation.ManufacturerId;

        await _validator.ValidateAsync(itemCategoryId);

        if (manufacturerId != null)
        {
            await _validator.ValidateAsync(manufacturerId.Value);
        }

        _cancellationToken.ThrowIfCancellationRequested();

        var availabilities = creation.Availabilities;
        await _validator.ValidateAsync(availabilities);

        var storeItem = _storeItemFactory.Create(creation);

        await _itemRepository.StoreAsync(storeItem, _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();
    }

    public async Task CreateItemWithTypesAsync(IStoreItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        await _itemRepository.StoreAsync(item, _cancellationToken);
    }
}