using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;

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
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var result = _quantitiesQueryService.GetAllQuantityTypesInPacket();

        return Task.FromResult(result);
    }
}