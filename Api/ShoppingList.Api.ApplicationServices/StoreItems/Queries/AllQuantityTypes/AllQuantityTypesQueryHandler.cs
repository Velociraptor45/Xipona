using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.AllQuantityTypes;

public class AllQuantityTypesQueryHandler : IQueryHandler<AllQuantityTypesQuery, IEnumerable<QuantityTypeReadModel>>
{
    private readonly IQuantitiesQueryService _quantitiesQueryService;

    public AllQuantityTypesQueryHandler(IQuantitiesQueryService quantitiesQueryService)
    {
        _quantitiesQueryService = quantitiesQueryService;
    }

    public Task<IEnumerable<QuantityTypeReadModel>> HandleAsync(AllQuantityTypesQuery query,
        CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var result = _quantitiesQueryService.GetAllQuantityTypes();

        return Task.FromResult(result);
    }
}