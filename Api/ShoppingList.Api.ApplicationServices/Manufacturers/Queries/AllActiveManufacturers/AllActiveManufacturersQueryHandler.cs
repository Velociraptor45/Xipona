using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;

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
        ArgumentNullException.ThrowIfNull(query);

        var service = _manufacturerQueryServiceDelegate(cancellationToken);
        return await service.GetAllActive();
    }
}