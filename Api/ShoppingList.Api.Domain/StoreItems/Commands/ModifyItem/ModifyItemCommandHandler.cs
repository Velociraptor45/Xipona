using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem
{
    public class ModifyItemCommandHandler : ICommandHandler<ModifyItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreItemSectionReadRepository storeItemSectionReadRepository;
        private readonly IStoreRepository storeRepository;

        public ModifyItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, IStoreItemAvailabilityFactory storeItemAvailabilityFactory,
            IStoreItemSectionReadRepository storeItemSectionReadRepository, IStoreRepository storeRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemSectionReadRepository = storeItemSectionReadRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(ModifyItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var storeItem = await itemRepository.FindByAsync(command.ItemModify.Id, cancellationToken);

            if (storeItem == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemModify.Id));
            if (storeItem.IsTemporary)
                throw new DomainException(new TemporaryItemNotModifyableReason(command.ItemModify.Id));

            cancellationToken.ThrowIfCancellationRequested();

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemModify.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(command.ItemModify.ItemCategoryId));

            IManufacturer manufacturer = null;
            if (command.ItemModify.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.ItemModify.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new DomainException(new ManufacturerNotFoundReason(command.ItemModify.ManufacturerId));
            }

            IEnumerable<IStoreItemAvailability> availabilities =
                await GetStoreItemAvailabilities(command.ItemModify.Availabilities, cancellationToken);
            storeItem.Modify(command.ItemModify, itemCategory, manufacturer, availabilities);
            var availableAtStores = storeItem.Availabilities.Select(av => av.StoreId);

            var shoppingListsWithItem = (await shoppingListRepository.FindByAsync(storeItem.Id, cancellationToken))
                .Where(list => !availableAtStores.Any(store => store.Id == list.Store.Id.AsStoreItemStoreId()))
                .ToList();

            using var transaction = await transactionGenerator.GenerateAsync(cancellationToken);

            await itemRepository.StoreAsync(storeItem, cancellationToken);
            foreach (var list in shoppingListsWithItem)
            {
                // remove items from all shopping lists where item is not available anymore
                list.RemoveItem(storeItem.Id.ToShoppingListItemId());
                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

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

                StoreId storeId = shortAvailability.StoreId.AsStoreId();
                var store = await storeRepository.FindActiveByAsync(storeId, cancellationToken);
                if (store == null)
                    throw new DomainException(new StoreNotFoundReason(storeId));

                var availability = storeItemAvailabilityFactory
                    .Create(store, shortAvailability.Price, shortAvailability.StoreItemSectionId);
                availabilities.Add(availability);
            }

            return availabilities;
        }
    }
}