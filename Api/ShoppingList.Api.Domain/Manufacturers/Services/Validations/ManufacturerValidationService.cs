using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

public class ManufacturerValidationService : IManufacturerValidationService
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerValidationService(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task ValidateAsync(ManufacturerId manufacturerId)
    {
        IManufacturer? manufacturer = await _manufacturerRepository
            .FindActiveByAsync(manufacturerId);

        if (manufacturer == null)
            throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));
    }
}