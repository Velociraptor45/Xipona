using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.RecipeTags.Models;
using Xipona.Api.Domain.RecipeTags.Services.Query;

namespace Xipona.Api.ApplicationServices.RecipeTags.Queries.GetAll;

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