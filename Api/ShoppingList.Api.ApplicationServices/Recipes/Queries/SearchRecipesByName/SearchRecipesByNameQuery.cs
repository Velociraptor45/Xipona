using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;

public class SearchRecipesByNameQuery : IQuery<IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByNameQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}