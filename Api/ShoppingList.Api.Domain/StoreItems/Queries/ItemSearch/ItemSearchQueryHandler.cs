using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch
{
    public class ItemSearchQueryHandler : IQueryHandler<ItemSearchQuery, IEnumerable<ItemSearchReadModel>>
    {
        private readonly IItemRepository itemRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ItemSearchQueryHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.storeRepository = storeRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> HandleAsync(ItemSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (string.IsNullOrWhiteSpace(query.SearchInput))
                return Enumerable.Empty<ItemSearchReadModel>();

            var store = await storeRepository.FindByAsync(new Stores.Models.StoreId(query.StoreId.Value), cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(query.StoreId));

            IEnumerable<IStoreItem> storeItems = await itemRepository
                .FindActiveByAsync(query.SearchInput.Trim(), query.StoreId, cancellationToken);
            IShoppingList shoppingList = await shoppingListRepository
                .FindActiveByAsync(query.StoreId, cancellationToken);
            var itemIdsOnShoppingList = shoppingList.Items.Select(item => item.Id);

            var itemsNotOnShoppingList = storeItems
                .Where(item => !itemIdsOnShoppingList.Contains(item.Id));

            var itemCategoryIds = storeItems.Select(i => i.ItemCategoryId).Distinct();
            var itemCategoryDict = (await itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
                .ToDictionary(i => i.Id);

            var manufacturerIds = storeItems.Select(i => i.ManufacturerId).Distinct();
            var manufaturerDict = (await manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
                .ToDictionary(m => m.Id);

            return itemsNotOnShoppingList
                .Select(item =>
                {
                    IManufacturer manufacturer = item.ManufacturerId == null ? null : manufaturerDict[item.ManufacturerId];
                    IItemCategory category = item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId];

                    IStoreItemAvailability storeAvailability = item.Availabilities
                        .Single(av => av.StoreId == query.StoreId);

                    var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                    return item.ToItemSearchReadModel(query.StoreId, category, manufacturer, section, storeAvailability);
                });
        }
    }
}