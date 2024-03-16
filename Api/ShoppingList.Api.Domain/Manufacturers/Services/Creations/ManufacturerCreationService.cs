using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;

public class ManufacturerCreationService : IManufacturerCreationService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IManufacturerFactory _manufacturerFactory;

    public ManufacturerCreationService(IManufacturerRepository manufacturerRepository,
        IManufacturerFactory manufacturerFactory)
    {
        _manufacturerRepository = manufacturerRepository;
        _manufacturerFactory = manufacturerFactory;
    }

    public async Task<IManufacturer> CreateAsync(ManufacturerName name)
    {
        var model = _manufacturerFactory.Create(name);
        return await _manufacturerRepository.StoreAsync(model);
    }
}