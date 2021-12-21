using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services
{
    public class ItemQueryService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IItemTypeReadRepository _itemTypeReadRepository;
        private readonly IItemSearchReadModelConversionService _itemSearchReadModelConversionService;
        private readonly CancellationToken _cancellationToken;

        public ItemQueryService(IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository, IItemTypeReadRepository itemTypeReadRepository,
            IItemSearchReadModelConversionService itemSearchReadModelConversionService,
            CancellationToken cancellationToken)
        {
            _itemRepository = itemRepository;
            _shoppingListRepository = shoppingListRepository;
            _storeRepository = storeRepository;
            _itemTypeReadRepository = itemTypeReadRepository;
            _itemSearchReadModelConversionService = itemSearchReadModelConversionService;
            _cancellationToken = cancellationToken;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> SearchAsync(string name, StoreId storeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (storeId is null)
                throw new ArgumentNullException(nameof(storeId));

            var store = await _storeRepository.FindByAsync(storeId, _cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(storeId));

            var searchResultItemGroups = (await _itemRepository
                .FindActiveByAsync(name.Trim(), storeId, _cancellationToken))
                .ToLookup(i => i.HasItemTypes);
            IShoppingList? shoppingList = await _shoppingListRepository
                .FindActiveByAsync(storeId, _cancellationToken);
            if (shoppingList is null)
                throw new DomainException(new ShoppingListNotFoundReason(storeId));

            var itemIdsOnShoppingListGroups = shoppingList.Items
                .Select(item => (item.Id, item.TypeId))
                .ToLookup(tuple => tuple.TypeId == null);

            // items without types
            var searchResultItemsWithoutTypes = searchResultItemGroups[false];
            var itemsWithoutTypesOnShoppingList = itemIdsOnShoppingListGroups[true].Select(tuple => tuple.Id);
            var itemsWithoutTypesNotOnShoppingList = searchResultItemsWithoutTypes
                .Where(item => !itemsWithoutTypesOnShoppingList.Contains(item.Id));

            var itemWithoutTypesReadModels = await _itemSearchReadModelConversionService.ConvertAsync(
                itemsWithoutTypesNotOnShoppingList, store, _cancellationToken);

            // items with types
            var searchResultItemsWithTypesDict = searchResultItemGroups[true].ToDictionary(g => g.Id);
            var itemsWithTypesOnShoppingList = itemIdsOnShoppingListGroups[false].ToLookup(g => g.Id, g => g.TypeId!);
            var itemsWithTypeNotOnShoppingList = new List<(IStoreItem, IEnumerable<ItemTypeId>)>();
            foreach (var itemOnShoppingList in itemsWithTypesOnShoppingList)
            {
                if (!searchResultItemsWithTypesDict.TryGetValue(itemOnShoppingList.Key, out var searchResultItem))
                    continue;

                var typesOnList = itemOnShoppingList.ToList();
                var searchResultItemTypeIdsNotOnShoppingList = searchResultItem.ItemTypes
                    .Where(t => t.Availabilities.Any(av => av.StoreId == storeId))
                    .Select(t => t.Id)
                    .Intersect(typesOnList)
                    .ToList();

                if (!searchResultItemTypeIdsNotOnShoppingList.Any())
                    continue;

                itemsWithTypeNotOnShoppingList.Add((searchResultItem, searchResultItemTypeIdsNotOnShoppingList));
            }

            var itemsWithTypesReadModels = await _itemSearchReadModelConversionService.ConvertAsync(
                itemsWithTypeNotOnShoppingList, store, _cancellationToken);

            // todo items with types

            return;
        }
    }
}