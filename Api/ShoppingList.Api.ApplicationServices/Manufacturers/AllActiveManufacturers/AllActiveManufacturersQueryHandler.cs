using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.AllActiveManufacturers;

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
        return await service.GetAllActive();
    }
}