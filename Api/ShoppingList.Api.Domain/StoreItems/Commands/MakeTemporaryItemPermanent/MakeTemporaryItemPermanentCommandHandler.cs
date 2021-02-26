using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using StoreModels = ProjectHermes.ShoppingList.Api.Domain.Stores.Model;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent
{
    public class MakeTemporaryItemPermanentCommandHandler : ICommandHandler<MakeTemporaryItemPermanentCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreItemSectionReadRepository storeItemSectionReadRepository;

        public MakeTemporaryItemPermanentCommandHandler(IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository,
            IStoreRepository storeRepository, IStoreItemAvailabilityFactory storeItemAvailabilityFactory,
            IStoreItemSectionReadRepository storeItemSectionReadRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.storeRepository = storeRepository;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemSectionReadRepository = storeItemSectionReadRepository;
        }

        public async Task<bool> HandleAsync(MakeTemporaryItemPermanentCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            IStoreItem storeItem = await itemRepository.FindByAsync(command.PermanentItem.Id, cancellationToken);
            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(command.PermanentItem.Id));
            if (!storeItem.IsTemporary)
                throw new DomainException(new ItemNotTemporaryReason(command.PermanentItem.Id));

            var itemCategory = await itemCategoryRepository
                .FindByAsync(command.PermanentItem.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(command.PermanentItem.ItemCategoryId));

            cancellationToken.ThrowIfCancellationRequested();

            IManufacturer manufacturer = null;
            if (command.PermanentItem.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.PermanentItem.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new DomainException(new ManufacturerNotFoundReason(command.PermanentItem.ManufacturerId));
            }

            IEnumerable<StoreModels.IStore> activeStores = await storeRepository.GetAsync(cancellationToken);
            foreach (var availability in command.PermanentItem.Availabilities)
            {
                if (!activeStores.Any(s => s.Id == availability.StoreId))
                    throw new DomainException(new StoreNotFoundReason(availability.StoreId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<IStoreItemAvailability> availabilities =
                await GetStoreItemAvailabilities(command.PermanentItem.Availabilities, cancellationToken);
            storeItem.MakePermanent(command.PermanentItem, itemCategory, manufacturer, availabilities);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            return true;
        }

        private async Task<IEnumerable<IStoreItemAvailability>> GetStoreItemAvailabilities(
            IEnumerable<ShortAvailability> shortAvailabilities, CancellationToken cancellationToken)
        {
            var sectionIds = shortAvailabilities.Select(av => av.StoreItemSectionId);
            var sections = (await storeItemSectionReadRepository.FindByAsync(sectionIds, cancellationToken))
                .ToLookup(s => s.Id);

            var availabilities = new List<IStoreItemAvailability>();
            foreach (var shortAvailability in shortAvailabilities)
            {
                if (!sections.Contains(shortAvailability.StoreItemSectionId))
                    throw new DomainException(new StoreItemSectionNotFoundReason(shortAvailability.StoreItemSectionId));
                var section = sections[shortAvailability.StoreItemSectionId].First();
                var store = await storeRepository.FindActiveByAsync(shortAvailability.StoreId.AsStoreId(), cancellationToken);
                var availability = storeItemAvailabilityFactory
                    .Create(store, shortAvailability.Price, section);
                availabilities.Add(availability);
            }

            return availabilities;
        }
    }
}