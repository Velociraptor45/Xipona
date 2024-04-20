using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Conversion.ItemReadModels;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Creations;

public class ItemCreationService : IItemCreationService
{
    private readonly IItemRepository _itemRepository;
    private readonly IValidator _validator;
    private readonly IItemFactory _itemFactory;
    private readonly IItemReadModelConversionService _conversionService;

    public ItemCreationService(IItemRepository itemRepository, IValidator validator, IItemFactory itemFactory,
        IItemReadModelConversionService conversionService)
    {
        _itemRepository = itemRepository;
        _itemFactory = itemFactory;
        _conversionService = conversionService;
        _validator = validator;
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