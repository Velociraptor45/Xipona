using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Manufacturers.Services.Queries;

namespace Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;

public class ManufacturerSearchQueryHandler
    : IQueryHandler<ManufacturerSearchQuery, IEnumerable<ManufacturerSearchResultReadModel>>
{
    private readonly Func<CancellationToken, IManufacturerQueryService> _manufacturerQueryServiceDelegate;

    public ManufacturerSearchQueryHandler(
        Func<CancellationToken, IManufacturerQueryService> manufacturerQueryServiceDelegate)
    {
        _manufacturerQueryServiceDelegate = manufacturerQueryServiceDelegate;
    }

    public async Task<IEnumerable<ManufacturerSearchResultReadModel>> HandleAsync(ManufacturerSearchQuery query,
        CancellationToken cancellationToken)
    {
        var service = _manufacturerQueryServiceDelegate(cancellationToken);
        return await service.SearchAsync(query.SearchInput, query.IncludeDeleted);
    }
}