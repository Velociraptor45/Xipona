using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Commands.CreateManufacturer;

public class CreateManufacturerCommandHandler : ICommandHandler<CreateManufacturerCommand, bool>
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IManufacturerFactory _manufacturerFactory;

    public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository,
        IManufacturerFactory manufacturerFactory)
    {
        _manufacturerRepository = manufacturerRepository;
        _manufacturerFactory = manufacturerFactory;
    }

    public async Task<bool> HandleAsync(CreateManufacturerCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var model = _manufacturerFactory.Create(ManufacturerId.New, command.Name, false);
        await _manufacturerRepository.StoreAsync(model, cancellationToken);
        return true;
    }
}