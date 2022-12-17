using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;

public class ManufacturerModificationService : IManufacturerModificationService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly CancellationToken _cancellationToken;

    public ManufacturerModificationService(
        IManufacturerRepository manufacturerRepository,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task ModifyAsync(ManufacturerModification modification)
    {
        var manufacturer = await _manufacturerRepository.FindActiveByAsync(modification.Id, _cancellationToken);
        if (manufacturer is null)
            throw new DomainException(new ManufacturerNotFoundReason(modification.Id));

        manufacturer.Modify(modification);

        await _manufacturerRepository.StoreAsync(manufacturer, _cancellationToken);
    }
}