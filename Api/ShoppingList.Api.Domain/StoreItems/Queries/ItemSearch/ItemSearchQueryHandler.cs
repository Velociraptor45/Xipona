using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ShoppingList.Api.Domain.Models;
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
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;

        public ItemSearchQueryHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> HandleAsync(ItemSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (string.IsNullOrWhiteSpace(query.SearchInput))
                return Enumerable.Empty<ItemSearchReadModel>();

            IEnumerable<StoreItem> storeItems = await itemRepository
                .FindByAsync(query.SearchInput.Trim(), query.StoreId, cancellationToken);
            Domain.ShoppingLists.Models.ShoppingList shoppingList = await shoppingListRepository
                .FindActiveByAsync(query.StoreId, cancellationToken);
            var itemIdsOnShoppingList = shoppingList.Items.Select(item => item.Id);

            var itemsNotOnShoppingList = storeItems
                .Where(item => !itemIdsOnShoppingList.Contains(item.Id.ToShoppingListItemId()));

            return itemsNotOnShoppingList
                .Select(item => item.ToItemSearchReadModel(query.StoreId));
        }
    }
}