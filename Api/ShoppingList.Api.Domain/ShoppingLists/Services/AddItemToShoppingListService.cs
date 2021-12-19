using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services
{
    public class AddItemToShoppingListService : IAddItemToShoppingListService
    {
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;
        private readonly IStoreRepository storeRepository;
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListItemFactory shoppingListItemFactory;

        public AddItemToShoppingListService(IShoppingListSectionFactory shoppingListSectionFactory,
            IStoreRepository storeRepository, IItemRepository itemRepository,
            IShoppingListItemFactory shoppingListItemFactory)
        {
            this.shoppingListSectionFactory = shoppingListSectionFactory;
            this.storeRepository = storeRepository;
            this.itemRepository = itemRepository;
            this.shoppingListItemFactory = shoppingListItemFactory;
        }

        public async Task AddItemToShoppingList(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
            float quantity, CancellationToken cancellationToken)
        {
            if (shoppingList is null)
                throw new ArgumentNullException(nameof(shoppingList));
            if (itemId is null)
                throw new ArgumentNullException(nameof(itemId));

            IStoreItem storeItem = await LoadItem(itemId, cancellationToken);
            await AddItemToShoppingList(shoppingList, storeItem, sectionId, quantity, cancellationToken);
        }

        public async Task AddItemToShoppingList(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
            SectionId? sectionId, float quantity, CancellationToken cancellationToken)
        {
            if (shoppingList is null)
                throw new ArgumentNullException(nameof(shoppingList));
            if (temporaryItemId is null)
                throw new ArgumentNullException(nameof(temporaryItemId));

            IStoreItem storeItem = await LoadItem(temporaryItemId, cancellationToken);
            await AddItemToShoppingList(shoppingList, storeItem, sectionId, quantity, cancellationToken);
        }

        private async Task<IStoreItem> LoadItem(ItemId itemId, CancellationToken cancellationToken)
        {
            IStoreItem? item = await itemRepository.FindByAsync(itemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));

            return item;
        }

        private async Task<IStoreItem> LoadItem(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
        {
            IStoreItem? item = await itemRepository.FindByAsync(temporaryItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryItemId));

            return item;
        }

        private IShoppingListItem CreateShoppingListItem(ItemId itemId, ItemTypeId? itemTypeId, float quantity)
        {
            return shoppingListItemFactory.Create(itemId, itemTypeId, false, quantity);
        }

        private void ValidateItemIsAvailableAtStore(IStoreItem storeItem, StoreId storeId,
            out IStoreItemAvailability availability)
        {
            availability = storeItem.Availabilities.FirstOrDefault(av => av.StoreId == storeId);
            if (availability == null)
                throw new DomainException(new ItemAtStoreNotAvailableReason(storeItem.Id, storeId));
        }

        internal async Task AddItemToShoppingList(IShoppingList shoppingList, IStoreItem storeItem,
            SectionId? sectionId, float quantity, CancellationToken cancellationToken)
        {
            if (storeItem.HasItemTypes)
                throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(storeItem.Id));

            ValidateItemIsAvailableAtStore(storeItem, shoppingList.StoreId, out var availability);

            if (sectionId == null)
                sectionId = availability.DefaultSectionId;

            cancellationToken.ThrowIfCancellationRequested();

            IShoppingListItem shoppingListItem = CreateShoppingListItem(storeItem.Id, null, quantity);
            await AddItemToShoppingList(shoppingList, shoppingListItem, sectionId, cancellationToken);
        }

        internal async Task AddItemToShoppingList(IShoppingList shoppingList, IShoppingListItem item,
            SectionId sectionId, CancellationToken cancellationToken)
        {
            var store = await storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

            if (!store.ContainsSection(sectionId))
                throw new DomainException(new SectionInStoreNotFoundReason(sectionId, store.Id));

            cancellationToken.ThrowIfCancellationRequested();

            if (!shoppingList.Sections.Any(s => s.Id == sectionId))
            {
                var section = shoppingListSectionFactory.CreateEmpty(sectionId);
                shoppingList.AddSection(section);
            }

            shoppingList.AddItem(item, sectionId);
        }
    }
}