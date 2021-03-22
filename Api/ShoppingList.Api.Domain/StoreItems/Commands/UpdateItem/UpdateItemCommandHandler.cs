using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;
        private readonly IShoppingListItemFactory shoppingListItemFactory;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IStoreItemSectionReadRepository storeItemSectionReadRepository;
        private readonly IStoreRepository storeRepository;

        public UpdateItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, IShoppingListItemFactory shoppingListItemFactory,
            IStoreItemAvailabilityFactory storeItemAvailabilityFactory, IStoreItemFactory storeItemFactory,
            IStoreItemSectionReadRepository storeItemSectionReadRepository, IStoreRepository storeRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
            this.shoppingListItemFactory = shoppingListItemFactory;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeItemFactory = storeItemFactory;
            this.storeItemSectionReadRepository = storeItemSectionReadRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            IStoreItem oldItem = await itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
            if (oldItem == null)
                throw new DomainException(new ItemNotFoundReason(command.ItemUpdate.OldId));
            if (oldItem.IsTemporary)
                throw new DomainException(new TemporaryItemNotUpdateableReason(command.ItemUpdate.OldId));

            oldItem.Delete();

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemUpdate.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(command.ItemUpdate.ItemCategoryId));

            IManufacturer manufacturer = null;
            if (command.ItemUpdate.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.ItemUpdate.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new DomainException(new ManufacturerNotFoundReason(command.ItemUpdate.ManufacturerId));
            }

            using ITransaction transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemRepository.StoreAsync(oldItem, cancellationToken);

            // create new Item
            IEnumerable<IStoreItemAvailability> availabilities =
                await GetStoreItemAvailabilities(command.ItemUpdate.Availabilities, cancellationToken);
            IStoreItem updatedItem = storeItemFactory
                .Create(command.ItemUpdate, itemCategory, manufacturer, oldItem, availabilities);
            updatedItem = await itemRepository.StoreAsync(updatedItem, cancellationToken);

            // change existing item references on shopping lists
            var shoppingListsWithOldItem = (await shoppingListRepository
                .FindActiveByAsync(command.ItemUpdate.OldId, cancellationToken))
                .ToList();

            foreach (var list in shoppingListsWithOldItem)
            {
                IShoppingListItem shoppingListItem = list.Items
                    .First(i => i.Id == command.ItemUpdate.OldId.ToShoppingListItemId());
                list.RemoveItem(shoppingListItem.Id);
                if (updatedItem.IsAvailableInStore(list.Store.Id.AsStoreItemStoreId()))
                {
                    var priceAtStore = updatedItem.Availabilities
                        .First(av => av.Store.Id == list.Store.Id)
                        .Price;

                    var section = updatedItem.GetDefaultSectionForStore(list.Store.Id.AsStoreItemStoreId());
                    var updatedListItem = shoppingListItemFactory.Create(updatedItem, priceAtStore,
                        shoppingListItem.IsInBasket, shoppingListItem.Quantity);
                    list.AddItem(updatedListItem, section.Id.ToShoppingListSectionId()); // todo extract this into service
                }

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