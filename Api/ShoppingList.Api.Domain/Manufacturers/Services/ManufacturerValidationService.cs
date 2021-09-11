using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services
{
    public class ManufacturerValidationService : IManufacturerValidationService
    {
        private readonly IManufacturerRepository manufacturerRepository;

        public ManufacturerValidationService(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task ValidateAsync(ManufacturerId manufacturerId, CancellationToken cancellationToken)
        {
            if (manufacturerId is null)
                throw new ArgumentNullException(nameof(manufacturerId));

            IManufacturer? manufacturer = await manufacturerRepository
                    .FindByAsync(manufacturerId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (manufacturer == null)
                throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));
        }
    }
}