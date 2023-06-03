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

    public ItemCreationService(
        Func<CancellationToken, IItemRepository> itemRepositoryDelegate,
        Func<CancellationToken, IValidator> validatorDelegate,
        IItemFactory itemFactory,
        Func<CancellationToken, IItemReadModelConversionService> conversionServiceDelegate,
        CancellationToken cancellationToken)
    {
        _itemRepository = itemRepositoryDelegate(cancellationToken);
        _itemFactory = itemFactory;
        _conversionService = conversionServiceDelegate(cancellationToken);
        _validator = validatorDelegate(cancellationToken);
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

        var availabilities = creation.Availabilities;
        await _validator.ValidateAsync(availabilities);

        var item = _itemFactory.Create(creation);

        var storedItem = await _itemRepository.StoreAsync(item);

        return await _conversionService.ConvertAsync(storedItem);
    }

    public async Task<ItemReadModel> CreateItemWithTypesAsync(IItem item)
    {
        var storedItem = await _itemRepository.StoreAsync(item);

        return await _conversionService.ConvertAsync(storedItem);
    }
}