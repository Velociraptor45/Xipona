using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

public class Validator : IValidator
{
    private readonly IAvailabilityValidationService _availabilityValidationService;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IItemValidationService _itemValidationService;
    private readonly IRecipeTagValidationService _recipeTagValidationService;

    public Validator(
        Func<CancellationToken, IAvailabilityValidationService> availabilityValidationServiceDelegate,
        Func<CancellationToken, IItemCategoryValidationService> itemCategoryValidationServiceDelegate,
        Func<CancellationToken, IManufacturerValidationService> manufacturerValidationServiceDelegate,
        Func<CancellationToken, IItemValidationService> itemValidationServiceDelegate,
        Func<CancellationToken, IRecipeTagValidationService> recipeTagValidationServiceDelegate,
        CancellationToken cancellationToken)
    {
        _availabilityValidationService = availabilityValidationServiceDelegate(cancellationToken);
        _itemCategoryValidationService = itemCategoryValidationServiceDelegate(cancellationToken);
        _manufacturerValidationService = manufacturerValidationServiceDelegate(cancellationToken);
        _itemValidationService = itemValidationServiceDelegate(cancellationToken);
        _recipeTagValidationService = recipeTagValidationServiceDelegate(cancellationToken);
    }

    public async Task ValidateAsync(IEnumerable<IItemAvailability> availabilities)
    {
        await _availabilityValidationService.ValidateAsync(availabilities);
    }

    public async Task ValidateAsync(ItemCategoryId itemCategoryId)
    {
        await _itemCategoryValidationService.ValidateAsync(itemCategoryId);
    }

    public async Task ValidateAsync(ManufacturerId manufacturerId)
    {
        await _manufacturerValidationService.ValidateAsync(manufacturerId);
    }

    public async Task ValidateAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        await _itemValidationService.ValidateAsync(itemId, itemTypeId);
    }

    public async Task ValidateAsync(IEnumerable<RecipeTagId> recipeTagIds)
    {
        await _recipeTagValidationService.ValidateAsync(recipeTagIds);
    }
}