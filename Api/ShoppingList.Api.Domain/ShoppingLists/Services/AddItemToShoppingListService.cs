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

        public async Task AddItemToShoppingList(IShoppingList shoppingList, ItemId itemId, SectionId sectionId,
            float quantity, CancellationToken cancellationToken)
        {
            IShoppingListItem item = await LoadItem(itemId, quantity, cancellationToken);
            await AddToShoppingList(shoppingList, item, sectionId, cancellationToken);
        }

        public async Task AddItemToShoppingList(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
            SectionId sectionId, float quantity, CancellationToken cancellationToken)
        {
            IShoppingListItem item = await LoadItem(temporaryItemId, quantity, cancellationToken);
            await AddToShoppingList(shoppingList, item, sectionId, cancellationToken);
        }

        private async Task<IShoppingListItem> LoadItem(ItemId itemId, float quantity, CancellationToken cancellationToken)
        {
            IStoreItem item = await itemRepository.FindByAsync(itemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(itemId));

            return shoppingListItemFactory.Create(item.Id, false, quantity);
        }

        private async Task<IShoppingListItem> LoadItem(TemporaryItemId temporaryItemId, float quantity,
            CancellationToken cancellationToken)
        {
            IStoreItem item = await itemRepository.FindByAsync(temporaryItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(temporaryItemId));

            return shoppingListItemFactory.Create(item.Id, false, quantity);
        }

        private async Task AddToShoppingList(IShoppingList shoppingList, IShoppingListItem item, SectionId sectionId,
            CancellationToken cancellationToken)
        {
            if (shoppingList is null)
                throw new ArgumentNullException(nameof(shoppingList));
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var store = await storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

            if (sectionId == null)
            {
                // add to default section is no section is specified
                sectionId = store.GetDefaultSection().Id;
            }

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