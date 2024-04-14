using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Shared.Validations;

public class Validator : IValidator
{
    private readonly IAvailabilityValidationService _availabilityValidationService;
    private readonly IItemCategoryValidationService _itemCategoryValidationService;
    private readonly IManufacturerValidationService _manufacturerValidationService;
    private readonly IItemValidationService _itemValidationService;
    private readonly IRecipeTagValidationService _recipeTagValidationService;

    public Validator(
        IAvailabilityValidationService availabilityValidationService,
        IItemCategoryValidationService itemCategoryValidationService,
        IManufacturerValidationService manufacturerValidationService,
        IItemValidationService itemValidationService,
        IRecipeTagValidationService recipeTagValidationService)
    {
        _availabilityValidationService = availabilityValidationService;
        _itemCategoryValidationService = itemCategoryValidationService;
        _manufacturerValidationService = manufacturerValidationService;
        _itemValidationService = itemValidationService;
        _recipeTagValidationService = recipeTagValidationService;
    }

    public async Task ValidateAsync(IEnumerable<ItemAvailability> availabilities)
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