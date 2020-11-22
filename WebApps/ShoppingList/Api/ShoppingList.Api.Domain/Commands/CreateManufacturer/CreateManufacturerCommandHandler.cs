using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.CreateManufacturer
{
    public class CreateManufacturerCommandHandler : ICommandHandler<CreateManufacturerCommand, bool>
    {
        private readonly IManufacturerRepository manufacturerRepository;

        public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<bool> HandleAsync(CreateManufacturerCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var model = new Manufacturer(new ManufacturerId(0), command.Name, false);
            await manufacturerRepository.StoreAsync(model, cancellationToken);
            return true;
        }
    }
}