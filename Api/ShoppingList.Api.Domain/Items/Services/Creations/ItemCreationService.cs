using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;

public class ItemCreationService : IItemCreationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;
    private readonly IItemFactory _itemFactory;
    private readonly IItemReadModelConversionService _conversionService;
    private readonly CancellationToken _cancellationToken;

    public ItemCreationService(
        IItemRepository itemRepository,
        Func<CancellationToken, IValidator> validatorDelegate,
        IItemFactory itemFactory,
        Func<CancellationToken, IItemReadModelConversionService> conversionServiceDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepository;
        _itemFactory = itemFactory;
        _conversionService = conversionServiceDelegate(cancellationToken);
        _validator = validatorDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task<ItemReadModel> CreateAsync(ItemCreation creation)
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

        var item = _itemFactory.Create(creation);

        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem);
    }

    public async Task<ItemReadModel> CreateItemWithTypesAsync(IItem item)
    {
        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem);
    }

    public async Task<ItemReadModel> CreateTemporaryAsync(TemporaryItemCreation creation)
    {
        var availability = creation.Availability;
        await _validator.ValidateAsync(availability.ToMonoList());

        var item = _itemFactory.Create(creation);

        var storedItem = await _itemRepository.StoreAsync(item, _cancellationToken);

        return await _conversionService.ConvertAsync(storedItem);
    }
}