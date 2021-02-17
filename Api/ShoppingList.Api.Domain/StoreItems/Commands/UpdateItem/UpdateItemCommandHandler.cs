using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
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

        public UpdateItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator, IShoppingListItemFactory shoppingListItemFactory,
            IStoreItemAvailabilityFactory storeItemAvailabilityFactory, IStoreItemFactory storeItemFactory,
            IStoreItemSectionReadRepository storeItemSectionReadRepository)
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
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            IStoreItem oldItem = await itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
            if (oldItem.IsTemporary)
                throw new DomainException(new TemporaryItemNotUpdateableReason(command.ItemUpdate.OldId));

            oldItem.Delete();

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemUpdate.ItemCategoryId, cancellationToken);

            IManufacturer manufacturer = null;
            if (command.ItemUpdate.ManufacturerId != null)
                manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemUpdate.ManufacturerId, cancellationToken);

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
                if (updatedItem.IsAvailableInStore(list.Store.Id.ToStoreItemStoreId()))
                {
                    var priceAtStore = updatedItem.Availabilities
                        .First(av => av.StoreId == list.Store.Id)
                        .Price;

                    var section = updatedItem.GetDefaultSectionForStore(list.Store.Id.ToStoreItemStoreId());
                    var updatedListItem = shoppingListItemFactory.Create(updatedItem, priceAtStore,
                        shoppingListItem.IsInBasket, shoppingListItem.Quantity);
                    list.AddItem(updatedListItem, section.Id.ToShoppingListSectionId());
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
                var section = sections[shortAvailability.StoreItemSectionId].First();
                var availability = storeItemAvailabilityFactory
                    .Create(shortAvailability.StoreId, shortAvailability.Price, section);
                availabilities.Add(availability);
            }

            return availabilities;
        }
    }
}