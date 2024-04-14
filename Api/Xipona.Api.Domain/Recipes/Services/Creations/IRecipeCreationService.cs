using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

public interface IRecipeCreationService
{
    Task<RecipeReadModel> CreateAsync(RecipeCreation creation);
}