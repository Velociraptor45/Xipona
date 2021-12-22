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
    public class ItemQueryService : IItemQueryService
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
            if (storeId is null)
                throw new ArgumentNullException(nameof(storeId));
            if (string.IsNullOrWhiteSpace(name))
                return Enumerable.Empty<ItemSearchReadModel>();

            var store = await LoadStoreAsync(storeId);

            var searchResultItemGroups = (await _itemRepository
                .FindActiveByAsync(name.Trim(), storeId, _cancellationToken))
                .ToLookup(i => i.HasItemTypes);
            IShoppingList? shoppingList = await LoadShoppingListAsync(storeId);

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
            var itemsWithTypeNotOnShoppingList = GetMatchingItemsWithTypeIds(storeId, searchResultItemsWithTypesDict,
                    itemIdsOnShoppingListGroups[false])
                .ToList();

            // types
            var itemsWithMatchingItemTypes = await GetItemsWithMatchingItemTypeIdsAsync(name, storeId,
                searchResultItemsWithTypesDict);
            itemsWithTypeNotOnShoppingList.AddRange(itemsWithMatchingItemTypes);

            var itemsWithTypesReadModels = await _itemSearchReadModelConversionService.ConvertAsync(
                itemsWithTypeNotOnShoppingList, store, _cancellationToken);

            return itemsWithTypesReadModels.Union(itemWithoutTypesReadModels);
        }

        private IEnumerable<ItemWithMatchingItemTypeIds> GetMatchingItemsWithTypeIds(StoreId storeId,
            Dictionary<ItemId, IStoreItem> searchResultItemsWithTypesDict, IEnumerable<(ItemId, ItemTypeId?)> itemIdsOnShoppingList)
        {
            var itemsWithTypesOnShoppingList = itemIdsOnShoppingList.ToLookup(g => g.Item1, g => g.Item2!);
            var result = new List<ItemWithMatchingItemTypeIds>();
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

                result.Add(
                    new ItemWithMatchingItemTypeIds(searchResultItem, searchResultItemTypeIdsNotOnShoppingList));
            }

            return result;
        }

        private async Task<IEnumerable<ItemWithMatchingItemTypeIds>> GetItemsWithMatchingItemTypeIdsAsync(
            string name, StoreId storeId, Dictionary<ItemId, IStoreItem> searchResultItemsWithTypesDict)
        {
            var itemTypeIdMappings = await _itemTypeReadRepository.FindActiveByAsync(name, storeId, _cancellationToken);
            var itemTypeIdGroups = itemTypeIdMappings
                .GroupBy(mapping => mapping.Item1, mapping => mapping.Item2)
                // de-duplication => sort out all items that were already considered
                .Where(group => !searchResultItemsWithTypesDict.ContainsKey(group.Key))
                .ToList();

            var itemIds = itemTypeIdGroups.Select(group => group.Key);
            var itemsDict = (await _itemRepository.FindByAsync(itemIds, _cancellationToken))
                .ToDictionary(i => i.Id);

            var result = new List<ItemWithMatchingItemTypeIds>();
            foreach (var itemTypeIdGroup in itemTypeIdGroups)
            {
                if (!itemsDict.TryGetValue(itemTypeIdGroup.Key, out var item))
                    throw new DomainException(new ItemNotFoundReason(itemTypeIdGroup.Key));

                result.Add(new ItemWithMatchingItemTypeIds(item, itemTypeIdGroup));
            }

            return result;
        }

        private async Task<IShoppingList> LoadShoppingListAsync(StoreId storeId)
        {
            IShoppingList? shoppingList = await _shoppingListRepository
                .FindActiveByAsync(storeId, _cancellationToken);
            if (shoppingList is null)
                throw new DomainException(new ShoppingListNotFoundReason(storeId));

            return shoppingList;
        }

        private async Task<IStore> LoadStoreAsync(StoreId storeId)
        {
            var store = await _storeRepository.FindByAsync(storeId, _cancellationToken);
            if (store == null)
                throw new DomainException(new StoreNotFoundReason(storeId));

            return store;
        }
    }
}