using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;

public class SearchRecipesByTagsQueryHandler : IQueryHandler<SearchRecipesByTagsQuery, IEnumerable<RecipeSearchResult>>
{
    private readonly Func<CancellationToken, IRecipeQueryService> _recipeQueryServiceDelegate;

    public SearchRecipesByTagsQueryHandler(
        Func<CancellationToken, IRecipeQueryService> recipeQueryServiceDelegate)
    {
        _recipeQueryServiceDelegate = recipeQueryServiceDelegate;
    }

    public Task<IEnumerable<RecipeSearchResult>> HandleAsync(SearchRecipesByTagsQuery query, CancellationToken cancellationToken)
    {
        var recipeQueryService = _recipeQueryServiceDelegate(cancellationToken);
        return recipeQueryService.SearchByTagIdsAsync(query.TagIds);
    }
}