using ProjectHermes.Xipona.Api.Domain.ItemCategories.Ports;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Ports;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Shared;

public class RecipeConversionService : IRecipeConversionService
{
    private readonly IItemRepository _itemRepository;
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IRecipeReadRepository _recipeReadRepository;

    public RecipeConversionService(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
        IRecipeReadRepository recipeReadRepository)
    {
        _itemRepository = itemRepository;
        _itemCategoryRepository = itemCategoryRepository;
        _recipeReadRepository = recipeReadRepository;
    }

    public async Task<RecipeReadModel> ToReadModelAsync(IRecipe recipe)
    {
        var itemIds = recipe.Ingredients
            .Where(i => i.DefaultItemId is not null)
            .Select(i => i.DefaultItemId!.Value)
            .ToList();
        var itemCategoryIds = recipe.Ingredients
        .Where(i => i.DefaultItemId is null)
            .Select(i => i.ItemCategoryId)
            .ToList();
        var items = (await _itemRepository.FindByAsync(itemIds)).ToDictionary(i => i.Id);
        var itemCategories = (await _itemCategoryRepository.FindByAsync(itemCategoryIds)).ToDictionary(c => c.Id);

        var steps = recipe.PreparationSteps
            .Select(s => new PreparationStepReadModel(s.Id, s.Instruction, s.SortingIndex))
            .ToList();
        var ingredients = recipe.Ingredients
            .Select(i =>
            {
                string name;
                if (i.DefaultItemId is null)
                    name = itemCategories[i.ItemCategoryId].Name;
                else if (i.DefaultItemTypeId is null)
                    name = items[i.DefaultItemId.Value].Name;
                else
                {
                    var item = items[i.DefaultItemId.Value];
                    var type = item.ItemTypes.Single(t => t.Id == i.DefaultItemTypeId.Value);
                    name = $"{item.Name} {type.Name}";
                }

                return new IngredientReadModel(i.Id, name, i.ItemCategoryId, i.QuantityType, i.Quantity,
                    i.ShoppingListProperties);
            })
            .ToList();

        SideDishReadModel? sideDish = null;
        if (recipe.SideDishId is not null)
        {
            sideDish = await _recipeReadRepository.GetSideDishAsync(recipe.SideDishId.Value);
        }

        var readModel = new RecipeReadModel(
            recipe.Id,
            recipe.Name,
            recipe.NumberOfServings,
            ingredients,
            steps,
            recipe.Tags,
            sideDish);

        return readModel;
    }
}