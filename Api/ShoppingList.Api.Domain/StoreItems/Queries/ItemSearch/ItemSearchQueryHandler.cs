using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
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
        private readonly IItemSearchReadModelConversionService itemSearchReadModelConversionService;

        public ItemSearchQueryHandler(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository, IItemSearchReadModelConversionService itemSearchReadModelConversionService)
        {
            this.itemRepository = itemRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.storeRepository = storeRepository;
            this.itemSearchReadModelConversionService = itemSearchReadModelConversionService;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> HandleAsync(ItemSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (string.IsNullOrWhiteSpace(query.SearchInput))
                return Enumerable.Empty<ItemSearchReadModel>();

            var store = await storeRepository.FindByAsync(new StoreId(query.StoreId.Value), cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(query.StoreId));

            IEnumerable<IStoreItem> storeItems = await itemRepository
                .FindActiveByAsync(query.SearchInput.Trim(), query.StoreId, cancellationToken);
            IShoppingList? shoppingList = await shoppingListRepository
                .FindActiveByAsync(query.StoreId, cancellationToken);
            if (shoppingList is null)
                throw new DomainException(new ShoppingListNotFoundReason(query.StoreId));

            var itemIdsOnShoppingList = shoppingList.Items.Select(item => item.Id);

            var itemsNotOnShoppingList = storeItems
                .Where(item => !itemIdsOnShoppingList.Contains(item.Id));

            return await itemSearchReadModelConversionService.ConvertAsync(itemsNotOnShoppingList, store, cancellationToken);
        }
    }
}