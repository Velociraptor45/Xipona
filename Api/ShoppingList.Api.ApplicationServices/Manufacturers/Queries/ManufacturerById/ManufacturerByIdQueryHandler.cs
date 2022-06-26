using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;

public class ManufacturerByIdQueryHandler : IQueryHandler<ManufacturerByIdQuery, IManufacturer>
{
    private readonly Func<CancellationToken, IManufacturerQueryService> _manufacturerQueryServiceDelegate;

    public ManufacturerByIdQueryHandler(
        Func<CancellationToken, IManufacturerQueryService> manufacturerQueryServiceDelegate)
    {
        _manufacturerQueryServiceDelegate = manufacturerQueryServiceDelegate;
    }

    public async Task<IManufacturer> HandleAsync(ManufacturerByIdQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        var service = _manufacturerQueryServiceDelegate(cancellationToken);
        return await service.GetAsync(query.ManufacturerId);
    }
}