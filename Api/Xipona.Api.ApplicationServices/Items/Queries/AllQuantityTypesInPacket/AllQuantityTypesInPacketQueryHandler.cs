using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Queries.Quantities;

namespace Xipona.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;

public class AllQuantityTypesInPacketQueryHandler :
    IQueryHandler<AllQuantityTypesInPacketQuery, IEnumerable<QuantityTypeInPacketReadModel>>
{
    private readonly IQuantitiesQueryService _quantitiesQueryService;

    public AllQuantityTypesInPacketQueryHandler(IQuantitiesQueryService quantitiesQueryService)
    {
        _quantitiesQueryService = quantitiesQueryService;
    }

    public Task<IEnumerable<QuantityTypeInPacketReadModel>> HandleAsync(AllQuantityTypesInPacketQuery query,
        CancellationToken cancellationToken)
    {
        var result = _quantitiesQueryService.GetAllQuantityTypesInPacket();

        return Task.FromResult(result);
    }
}