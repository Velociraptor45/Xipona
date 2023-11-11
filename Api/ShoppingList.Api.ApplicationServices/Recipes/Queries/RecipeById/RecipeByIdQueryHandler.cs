using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.RecipeById;

public class RecipeByIdQueryHandler : IQueryHandler<RecipeByIdQuery, RecipeReadModel>
{
    private readonly Func<CancellationToken, IRecipeQueryService> _recipeQueryServiceDelegate;

    public RecipeByIdQueryHandler(
        Func<CancellationToken, IRecipeQueryService> recipeQueryServiceDelegate)
    {
        _recipeQueryServiceDelegate = recipeQueryServiceDelegate;
    }

    public Task<RecipeReadModel> HandleAsync(RecipeByIdQuery query, CancellationToken cancellationToken)
    {
        var service = _recipeQueryServiceDelegate(cancellationToken);
        return service.GetAsync(query.RecipeId);
    }
}