namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public interface IRecipeQueryService
{
    Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput);
}