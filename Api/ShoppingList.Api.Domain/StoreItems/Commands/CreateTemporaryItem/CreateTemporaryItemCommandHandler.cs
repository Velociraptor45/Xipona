using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Linq;
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

            ShortAvailability shortAvailability = command.TemporaryItemCreation.Availability;

            var store = await storeRepository.FindByAsync(shortAvailability.StoreId.AsStoreId(),
                cancellationToken);
            if (store == null || store.IsDeleted)
                throw new DomainException(new StoreNotFoundReason(shortAvailability.StoreId));

            var defaultSection = store.Sections.Single(s => s.IsDefaultSection);

            IStoreItemAvailability storeItemAvailability = storeItemAvailabilityFactory
                    .Create(store, shortAvailability.Price, defaultSection);

            var storeItem = storeItemFactory.Create(command.TemporaryItemCreation, storeItemAvailability);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            return true;
        }
    }
}