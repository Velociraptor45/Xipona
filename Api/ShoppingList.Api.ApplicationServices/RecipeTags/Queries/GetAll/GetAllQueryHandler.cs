using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Query;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.RecipeTags.Queries.GetAll;

public class GetAllQueryHandler : IQueryHandler<GetAllQuery, IEnumerable<IRecipeTag>>
{
    private readonly Func<CancellationToken, IRecipeTagQueryService> _recipeTagQueryServiceDelegate;

    public GetAllQueryHandler(
        Func<CancellationToken, IRecipeTagQueryService> recipeTagQueryServiceDelegate)
    {
        _recipeTagQueryServiceDelegate = recipeTagQueryServiceDelegate;
    }

    public async Task<IEnumerable<IRecipeTag>> HandleAsync(GetAllQuery query, CancellationToken cancellationToken)
    {
        var recipeTagQueryService = _recipeTagQueryServiceDelegate(cancellationToken);
        return await recipeTagQueryService.GetAllAsync();
    }
}