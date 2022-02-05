using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.ManufacturerSearch;

public class ManufacturerSearchQueryHandler
    : IQueryHandler<ManufacturerSearchQuery, IEnumerable<ManufacturerReadModel>>
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerSearchQueryHandler(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(ManufacturerSearchQuery query, CancellationToken cancellationToken)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var manufacturerModels = await _manufacturerRepository.FindByAsync(query.SearchInput,
            cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        return manufacturerModels.Select(model => model.ToReadModel());
    }
}