using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Recipes.Queries.ItemAmountsForOneServing;

public class ItemAmountsForOneServingQueryHandler
    : IQueryHandler<ItemAmountsForOneServingQuery, IEnumerable<ItemAmountForOneServing>>
{
    private readonly Func<CancellationToken, IRecipeQueryService> _recipeQueryServiceDelegate;

    public ItemAmountsForOneServingQueryHandler(
        Func<CancellationToken, IRecipeQueryService> recipeQueryServiceDelegate)
    {
        _recipeQueryServiceDelegate = recipeQueryServiceDelegate;
    }

    public Task<IEnumerable<ItemAmountForOneServing>> HandleAsync(ItemAmountsForOneServingQuery query,
        CancellationToken cancellationToken)
    {
        var service = _recipeQueryServiceDelegate(cancellationToken);
        return service.GetItemAmountsForOneServingAsync(query.RecipeId);
    }
}