using Xipona.Api.Domain.Common.Exceptions;
using Xipona.Api.Domain.Manufacturers.Ports;
using Xipona.Api.Domain.Manufacturers.Reasons;

namespace Xipona.Api.Domain.Manufacturers.Services.Modifications;

public class ManufacturerModificationService : IManufacturerModificationService
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerModificationService(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task ModifyAsync(ManufacturerModification modification)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(modification.Id);
        if (manufacturer is null)
            throw new DomainException(new ManufacturerNotFoundReason(modification.Id));

        manufacturer.Modify(modification);

        await _manufacturerRepository.StoreAsync(manufacturer);
    }
}