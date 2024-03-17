using Microsoft.Extensions.Logging;
using ProjectHermes.Xipona.Api.Core.Attributes;
using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Domain.Common.Exceptions;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Reasons;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Shared;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Ports;
using ProjectHermes.Xipona.Api.Domain.Stores.Reasons;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

public class RecipeQueryService : IRecipeQueryService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IRecipeConversionService _recipeConversionService;
    private readonly IStoreRepository _storeRepository;
    private readonly IQuantityTranslationService _quantityTranslationService;
    private readonly ILogger<RecipeQueryService> _logger;

    public RecipeQueryService(IRecipeRepository recipeRepository, IItemRepository itemRepository,
        IRecipeConversionService recipeConversionService, IStoreRepository storeRepository,
        IQuantityTranslationService quantityTranslationService, ILogger<RecipeQueryService> logger)
    {
        _recipeRepository = recipeRepository;
        _itemRepository = itemRepository;
        _recipeConversionService = recipeConversionService;
        _storeRepository = storeRepository;
        _quantityTranslationService = quantityTranslationService;
        _logger = logger;
    }

    public async Task<RecipeReadModel> GetAsync(RecipeId id)
    {
        var recipe = await LoadRecipeAsync(id);
        return await _recipeConversionService.ToReadModelAsync(recipe);
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<RecipeSearchResult>();

        var results = (await _recipeRepository.SearchByAsync(searchInput)).ToList();

        _logger.LogInformation(() => "Found {ResultCount} result(s) for input '{Input}'",
            results.Count, searchInput);

        return results;
    }

    public async Task<IEnumerable<RecipeSearchResult>> SearchByTagIdsAsync(IEnumerable<RecipeTagId> tagIds)
    {
        var tagIdsList = tagIds.ToList();

        if (!tagIdsList.Any())
            return Enumerable.Empty<RecipeSearchResult>();

        var results = (await _recipeRepository.FindByContainingAllAsync(tagIdsList)).ToList();
        return results.Select(r => new RecipeSearchResult(r.Id, r.Name));
    }

    public async Task<IEnumerable<ItemAmountForOneServing>> GetItemAmountsForOneServingAsync(RecipeId recipeId)
    {
        var recipe = await LoadRecipeAsync(recipeId);
        var itemIds = recipe.Ingredients.Select(i => i.DefaultItemId).Where(i => i is not null).Cast<ItemId>().ToList();
        var items = (await _itemRepository.FindByAsync(itemIds)).ToDictionary(i => i.Id);
        var storeIds = items.Values
            .Where(i => !i.HasItemTypes)
            .SelectMany(i => i.Availabilities)
            .Union(items.Values.Where(i => i.HasItemTypes).SelectMany(i => i.ItemTypes.SelectMany(t => t.Availabilities)))
            .Select(a => a.StoreId)
            .Distinct()
            .ToList();
        var stores = (await _storeRepository.FindActiveByAsync(storeIds)).ToDictionary(s => s.Id);

        return recipe.Ingredients
            .Where(i => i.DefaultItemId is not null)
            .Select(ingredient =>
            {
                if (!items.ContainsKey(ingredient.DefaultItemId!.Value))
                    throw new DomainException(new ItemNotFoundReason(ingredient.DefaultItemId!.Value));

                var item = items[ingredient.DefaultItemId!.Value];
                IItemType? itemType = null;
                if (ingredient.DefaultItemTypeId is not null)
                {
                    itemType = item.ItemTypes.FirstOrDefault(t => t.Id == ingredient.DefaultItemTypeId)
                        ?? throw new DomainException(new ItemTypeNotFoundReason(item.Id, ingredient.DefaultItemTypeId.Value));
                }

                var availabilities = itemType is null
                    ? item.Availabilities
                    : itemType.Availabilities;

                var availability = availabilities.FirstOrDefault(a => a.StoreId == ingredient.ShoppingListProperties!.DefaultStoreId);
                if (availability is null)
                    ThrowForItemNotAvailable(item, itemType, ingredient.ShoppingListProperties!.DefaultStoreId);

                var (quantityType, quantity) = _quantityTranslationService.NormalizeForOneServing(
                    recipe.NumberOfServings,
                    ingredient.QuantityType,
                    ingredient.Quantity,
                    item.ItemQuantity);

                var name = itemType is null
                    ? item.Name
                    : $"{item.Name} {itemType.Name}";

                return new ItemAmountForOneServing(
                    item.Id,
                    ingredient.DefaultItemTypeId,
                    name,
                    quantityType,
                    quantityType.GetAttribute<QuantityLabelAttribute>().QuantityLabel,
                    quantity,
                    ingredient.ShoppingListProperties!.DefaultStoreId,
                    ingredient.ShoppingListProperties.AddToShoppingListByDefault,
                    availabilities.Select(av =>
                    {
                        if (!stores.ContainsKey(av.StoreId))
                            throw new DomainException(new StoreNotFoundReason(av.StoreId));

                        var store = stores[av.StoreId];

                        return new ItemAmountForOneServingAvailability(store.Id, store.Name, av.Price);
                    }));
            });

        void ThrowForItemNotAvailable(IItem item, IItemType? itemType, StoreId storeId)
        {
            if (itemType is null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(item.Id, storeId));

            throw new DomainException(new ItemTypeAtStoreNotAvailableReason(itemType.Id, storeId));
        }
    }

    private async Task<IRecipe> LoadRecipeAsync(RecipeId id)
    {
        var recipe = await _recipeRepository.FindByAsync(id);
        if (recipe is null)
            throw new DomainException(new RecipeNotFoundReason(id));

        return recipe;
    }
}