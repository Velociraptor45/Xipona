using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public class ManufacturerQueryService : IManufacturerQueryService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly CancellationToken _cancellationToken;

    public ManufacturerQueryService(
        IManufacturerRepository manufacturerRepository,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task<IEnumerable<ManufacturerReadModel>> GetAllActive()
    {
        var manufacturers = await _manufacturerRepository.FindByAsync(false, _cancellationToken);

        return manufacturers.Select(m => new ManufacturerReadModel(m));
    }

    public async Task<IEnumerable<ManufacturerReadModel>> Get(string searchInput)
    {
        var manufacturerModels = await _manufacturerRepository.FindByAsync(searchInput,
            _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        return manufacturerModels.Select(model => new ManufacturerReadModel(model));
    }
}