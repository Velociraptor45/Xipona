using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
    {
        private readonly IItemCategoryValidationService itemCategoryValidationService;
        private readonly IManufacturerValidationService manufacturerValidationService;
        private readonly IAvailabilityValidationService availabilityValidationService;
        private readonly IItemRepository itemRepository;
        private readonly IStoreItemFactory storeItemFactory;

        public CreateItemCommandHandler(IItemCategoryValidationService itemCategoryValidationService,
            IManufacturerValidationService manufacturerValidationService,
            IAvailabilityValidationService availabilityValidationService,
            IItemRepository itemRepository, IStoreItemFactory storeItemFactory)
        {
            this.itemCategoryValidationService = itemCategoryValidationService;
            this.manufacturerValidationService = manufacturerValidationService;
            this.availabilityValidationService = availabilityValidationService;
            this.itemRepository = itemRepository;
            this.storeItemFactory = storeItemFactory;
        }

        public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var itemCategoryId = command.ItemCreation.ItemCategoryId;
            var manufacturerId = command.ItemCreation.ManufacturerId;

            await itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

            if (manufacturerId != null)
            {
                await manufacturerValidationService.ValidateAsync(manufacturerId.Value, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var availabilities = command.ItemCreation.Availabilities;
            await availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

            var storeItem = storeItemFactory.Create(command.ItemCreation);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }
    }
}