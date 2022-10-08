using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;

public class AllIngredientQuantityTypesQueryHandler :
    IQueryHandler<AllIngredientQuantityTypesQuery, IEnumerable<IngredientQuantityTypeReadModel>>
{
    private readonly IQuantitiesQueryService _quantitiesQueryService;

    public AllIngredientQuantityTypesQueryHandler(IQuantitiesQueryService quantitiesQueryService)
    {
        _quantitiesQueryService = quantitiesQueryService;
    }

    public Task<IEnumerable<IngredientQuantityTypeReadModel>> HandleAsync(
        AllIngredientQuantityTypesQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(_quantitiesQueryService.GetAllIngredientQuantityTypes());
    }
}