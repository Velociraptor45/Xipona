using Xipona.Api.Domain.Recipes.Services.Queries;

namespace Xipona.Api.Domain.Recipes.Services.Creations;

public interface IRecipeCreationService
{
    Task<RecipeReadModel> CreateAsync(RecipeCreation creation);
}