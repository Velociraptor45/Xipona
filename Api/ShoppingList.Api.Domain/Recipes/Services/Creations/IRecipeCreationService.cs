using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public interface IRecipeCreationService
{
    Task<RecipeReadModel> CreateAsync(RecipeCreation creation);
}