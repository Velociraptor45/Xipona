using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;

public class ManufacturerCreationService : IManufacturerCreationService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IManufacturerFactory _manufacturerFactory;

    public ManufacturerCreationService(
        Func<CancellationToken, IManufacturerRepository> manufacturerRepositoryDelegate,
        IManufacturerFactory manufacturerFactory,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepositoryDelegate(cancellationToken);
        _manufacturerFactory = manufacturerFactory;
    }

    public async Task<IManufacturer> CreateAsync(ManufacturerName name)
    {
        var model = _manufacturerFactory.Create(ManufacturerId.New, name, false);
        return await _manufacturerRepository.StoreAsync(model);
    }
}