using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreItemSectionReadRepository storeItemSectionReadRepository;

        public CreateTemporaryItemCommandHandler(IItemRepository itemRepository, IStoreRepository storeRepository,
            IStoreItemFactory storeItemFactory, IStoreItemAvailabilityFactory storeItemAvailabilityFactory,
            IStoreItemSectionReadRepository storeItemSectionReadRepository)
        {
            this.itemRepository = itemRepository;
            this.storeRepository = storeRepository;
            this.storeItemFactory = storeItemFactory;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemSectionReadRepository = storeItemSectionReadRepository;
        }

        public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var store = await storeRepository.FindByAsync(new Stores.Model.StoreId(command.TemporaryItemCreation.Availability.StoreId.Value),
                cancellationToken);
            if (store == null || store.IsDeleted)
                throw new DomainException(new StoreNotFoundReason(command.TemporaryItemCreation.Availability.StoreId));

            ShortAvailability shortAvailability = command.TemporaryItemCreation.Availability;
            IStoreItemSection section = await storeItemSectionReadRepository
                .FindByAsync(shortAvailability.StoreItemSectionId, cancellationToken);

            if (section == null)
                throw new DomainException(new StoreItemSectionNotFoundReason(shortAvailability.StoreItemSectionId));

            IStoreItemAvailability storeItemAvailability = storeItemAvailabilityFactory
                    .Create(shortAvailability.StoreId, shortAvailability.Price, section);

            var storeItem = storeItemFactory.Create(command.TemporaryItemCreation, storeItemAvailability);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}