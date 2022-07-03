using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

public class ItemCreationService : IItemCreationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;
    private readonly IItemFactory _storeItemFactory;
    private readonly IStoreItemReadModelConversionService _conversionService;
    private readonly CancellationToken _cancellationToken;

    public ItemCreationService(
        IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IItemFactory storeItemFactory,
        IStoreItemReadModelConversionService conversionService,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _storeItemFactory = storeItemFactory;
        _conversionService = conversionService;
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task<StoreItemReadModel> CreateAsync(ItemCreation creation)
    {
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

        var item = _storeItemFactory.Create(creation);

        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem, _cancellationToken);
    }

    public async Task<StoreItemReadModel> CreateItemWithTypesAsync(IItem item)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem, _cancellationToken);
    }

    public async Task<StoreItemReadModel> CreateTemporaryAsync(TemporaryItemCreation creation)
    {
        var availability = creation.Availability;
        await _validator.ValidateAsync(availability.ToMonoList());

        var item = _storeItemFactory.Create(creation);

        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem, _cancellationToken);
    }
}