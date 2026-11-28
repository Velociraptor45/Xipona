using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Manufacturers.Reasons;

namespace Xipona.Api.Domain.Manufacturers.Services.Validations;

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