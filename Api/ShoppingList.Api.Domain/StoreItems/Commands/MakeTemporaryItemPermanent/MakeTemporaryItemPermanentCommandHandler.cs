using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryValidationService itemCategoryValidationService;
        private readonly IManufacturerValidationService manufacturerValidationService;
        private readonly IAvailabilityValidationService availabilityValidationService;

        public MakeTemporaryItemPermanentCommandHandler(IItemRepository itemRepository,
            IItemCategoryValidationService itemCategoryValidationService,
            IManufacturerValidationService manufacturerValidationService,
            IAvailabilityValidationService availabilityValidationService)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryValidationService = itemCategoryValidationService;
            this.manufacturerValidationService = manufacturerValidationService;
            this.availabilityValidationService = availabilityValidationService;
        }

        public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            IStoreItem? storeItem = await itemRepository.FindByAsync(command.PermanentItem.Id, cancellationToken);
            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(command.PermanentItem.Id));
            if (!storeItem.IsTemporary)
                throw new DomainException(new ItemNotTemporaryReason(command.PermanentItem.Id));

            var itemCategoryId = command.PermanentItem.ItemCategoryId;
            var manufacturerId = command.PermanentItem.ManufacturerId;

            await itemCategoryValidationService.ValidateAsync(itemCategoryId, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            if (manufacturerId != null)
            {
                await manufacturerValidationService.ValidateAsync(manufacturerId, cancellationToken);
            }

            var availabilities = command.PermanentItem.Availabilities;
            await availabilityValidationService.ValidateAsync(availabilities, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            storeItem.MakePermanent(command.PermanentItem, availabilities);
            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }
    }
}