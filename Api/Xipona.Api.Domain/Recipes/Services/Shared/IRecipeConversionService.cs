using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Shared;

public interface IRecipeConversionService
{
    Task<RecipeReadModel> ToReadModelAsync(IRecipe recipe);
}