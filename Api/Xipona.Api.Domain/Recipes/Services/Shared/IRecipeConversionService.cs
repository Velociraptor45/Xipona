using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.Domain.Recipes.Services.Shared;

public interface IRecipeConversionService
{
    Task<RecipeReadModel> ToReadModelAsync(IRecipe recipe);
}