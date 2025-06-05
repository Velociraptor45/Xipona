using Microsoft.Extensions.Caching.Memory;
using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Queries.Quantities;

public class QuantitiesQueryService : IQuantitiesQueryService
{
    private readonly IMemoryCache _cache;

    public QuantitiesQueryService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public IEnumerable<QuantityTypeReadModel> GetAllQuantityTypes()
    {
        var values = Enum.GetValues<QuantityType>().ToList();
        var readModels = values.Select(v => new QuantityTypeReadModel(v, _cache));

        return readModels;
    }

    public IEnumerable<QuantityTypeInPacketReadModel> GetAllQuantityTypesInPacket()
    {
        var values = Enum.GetValues<QuantityTypeInPacket>().ToList();
        var readModels = values.Select(v => new QuantityTypeInPacketReadModel(v));

        return readModels;
    }
}