using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Commands.CreateManufacturer
{
    public class CreateManufacturerCommandHandler : ICommandHandler<CreateManufacturerCommand, bool>
    {
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IManufacturerFactory manufacturerFactory;

        public CreateManufacturerCommandHandler(IManufacturerRepository manufacturerRepository,
            IManufacturerFactory manufacturerFactory)
        {
            this.manufacturerRepository = manufacturerRepository;
            this.manufacturerFactory = manufacturerFactory;
        }

        public async Task<bool> HandleAsync(CreateManufacturerCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var model = manufacturerFactory.Create(ManufacturerId.New, command.Name, false);
            await manufacturerRepository.StoreAsync(model, cancellationToken);
            return true;
        }
    }
}