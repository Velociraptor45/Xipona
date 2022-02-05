using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.AllActiveManufacturers;

public class AllActiveManufacturersQueryHandler
    : IQueryHandler<AllActiveManufacturersQuery, IEnumerable<ManufacturerReadModel>>
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public AllActiveManufacturersQueryHandler(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(AllActiveManufacturersQuery query,
        CancellationToken cancellationToken)
    {
        var manufacturers = await _manufacturerRepository.FindByAsync(false, cancellationToken);

        return manufacturers.Select(m => m.ToReadModel());
    }
}