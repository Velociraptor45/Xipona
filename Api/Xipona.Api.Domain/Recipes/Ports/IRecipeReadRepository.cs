using Xipona.Api.Domain.Recipes.Models;
using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.Domain.Recipes.Ports;

public interface IRecipeReadRepository
{
    Task<SideDishReadModel?> GetSideDishAsync(RecipeId recipeId);
}