using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Ports;

public interface IRecipeReadRepository
{
    Task<SideDishReadModel?> GetSideDishAsync(RecipeId recipeId);
}