using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

public class ManufacturerModificationService : IManufacturerModificationService
{
    private readonly IManufacturerRepository _manufacturerRepository;

    public ManufacturerModificationService(
        Func<CancellationToken, IManufacturerRepository> manufacturerRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepositoryDelegate(cancellationToken);
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