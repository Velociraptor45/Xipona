using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;

public class SearchRecipesByNameQueryHandler : IQueryHandler<SearchRecipesByNameQuery, IEnumerable<RecipeSearchResult>>
{
    private readonly Func<CancellationToken, IRecipeQueryService> _recipeQueryServiceDelegate;

    public SearchRecipesByNameQueryHandler(Func<CancellationToken, IRecipeQueryService> recipeQueryServiceDelegate)
    {
        _recipeQueryServiceDelegate = recipeQueryServiceDelegate;
    }

    public async Task<IEnumerable<RecipeSearchResult>> HandleAsync(SearchRecipesByNameQuery query, CancellationToken cancellationToken)
    {
        var service = _recipeQueryServiceDelegate(cancellationToken);
        return await service.SearchByNameAsync(query.SearchInput);
    }
}