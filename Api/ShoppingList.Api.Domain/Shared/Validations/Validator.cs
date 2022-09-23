using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

public class Validator : IValidator
{
    private readonly IAvailabilityValidationService _availabilityValidationService;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IItemValidationService _itemValidationService;
    private readonly CancellationToken _cancellationToken;

    public Validator(IAvailabilityValidationService availabilityValidationService,
        IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        Func<CancellationToken, IItemValidationService> itemValidationServiceDelegate,
        CancellationToken cancellationToken)
    {
        _availabilityValidationService = availabilityValidationService;
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _itemValidationService = itemValidationServiceDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task ValidateAsync(IEnumerable<IItemAvailability> availabilities)
    {
        if (availabilities is null)
            throw new ArgumentNullException(nameof(availabilities));

        await _availabilityValidationService.ValidateAsync(availabilities, _cancellationToken);
    }

    public async Task ValidateAsync(ItemCategoryId itemCategoryId)
    {
        await _itemCategoryValidationService.ValidateAsync(itemCategoryId, _cancellationToken);
    }

    public async Task ValidateAsync(ManufacturerId manufacturerId)
    {
        await _manufacturerValidationService.ValidateAsync(manufacturerId, _cancellationToken);
    }

    public async Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        await _itemValidationService.ValidateAsync(itemId, itemTypeId);
    }
}