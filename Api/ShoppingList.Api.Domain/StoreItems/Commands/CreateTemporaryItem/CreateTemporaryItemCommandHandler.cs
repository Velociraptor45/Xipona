using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IAvailabilityValidationService availabilityValidationService;

        public CreateTemporaryItemCommandHandler(IItemRepository itemRepository, IStoreItemFactory storeItemFactory,
            IAvailabilityValidationService availabilityValidationService)
        {
            this.itemRepository = itemRepository;
            this.storeItemFactory = storeItemFactory;
            this.availabilityValidationService = availabilityValidationService;
        }

        public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var availability = command.TemporaryItemCreation.Availability;
            await availabilityValidationService.Validate(availability.ToMonoList(), cancellationToken);

            var storeItem = storeItemFactory.Create(command.TemporaryItemCreation);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}