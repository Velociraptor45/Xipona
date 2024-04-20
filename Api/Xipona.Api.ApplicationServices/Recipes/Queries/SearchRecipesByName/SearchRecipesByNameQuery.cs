using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;

public class SearchRecipesByNameQuery : IQuery<IEnumerable<RecipeSearchResult>>
{
    public SearchRecipesByNameQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}