using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;

public class ManufacturerValidationService : IManufacturerValidationService
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerValidationService(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task ValidateAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken)
    {
        IManufacturer? manufacturer = await _manufacturerRepository
            .FindByAsync(manufacturerId, cancellationToken);

        cancellationToken.ThrowIfCancellationRequested();

        if (manufacturer == null)
            throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));
    }
}