using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

public interface IRecipeQueryService
{
    Task<RecipeReadModel> GetAsync(RecipeId id);

    Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput);

    Task<IEnumerable<RecipeSearchResult>> SearchByTagIdsAsync(IEnumerable<RecipeTagId> tagIds);

    Task<IEnumerable<ItemAmountForOneServing>> GetItemAmountsForOneServingAsync(RecipeId recipeId);
}