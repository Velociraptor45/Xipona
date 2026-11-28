using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;

namespace Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypes;

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
        var result = _quantitiesQueryService.GetAllQuantityTypes();

        return Task.FromResult(result);
    }
}