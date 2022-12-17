using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;
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

    public async Task<IEnumerable<ManufacturerReadModel>> GetAllActiveAsync()
    {
        var manufacturers = await _manufacturerRepository.FindActiveByAsync(_cancellationToken);

        return manufacturers.Select(m => new ManufacturerReadModel(m));
    }

    public async Task<IEnumerable<ManufacturerSearchResultReadModel>> SearchAsync(string searchInput,
        bool includeDeleted)
    {
        if (string.IsNullOrWhiteSpace(searchInput))
            return Enumerable.Empty<ManufacturerSearchResultReadModel>();

        var manufacturerModels = await _manufacturerRepository.FindByAsync(searchInput, includeDeleted,
            _cancellationToken);

        _cancellationToken.ThrowIfCancellationRequested();

        return manufacturerModels.Select(model => new ManufacturerSearchResultReadModel(model.Id, model.Name));
    }

    public async Task<IManufacturer> GetAsync(ManufacturerId manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(manufacturerId, _cancellationToken);

        if (manufacturer is null)
            throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));

        return manufacturer;
    }
}