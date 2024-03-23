using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;

public class AllActiveManufacturersQueryHandler
    : IQueryHandler<AllActiveManufacturersQuery, IEnumerable<ManufacturerReadModel>>
{
    private readonly Func<CancellationToken, IManufacturerQueryService> _manufacturerQueryServiceDelegate;

    public AllActiveManufacturersQueryHandler(
        Func<CancellationToken, IManufacturerQueryService> manufacturerQueryServiceDelegate)
    {
        _manufacturerQueryServiceDelegate = manufacturerQueryServiceDelegate;
    }

    public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(AllActiveManufacturersQuery query,
        CancellationToken cancellationToken)
    {
        var service = _manufacturerQueryServiceDelegate(cancellationToken);
        return await service.GetAllActiveAsync();
    }
}