using ShoppingList.Api.Domain.Converters;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Queries.ItemSearch
{
    public class ItemSearchQueryHandler : IQueryHandler<ItemSearchQuery, IEnumerable<ItemSearchReadModel>>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ItemSearchQueryHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> HandleAsync(ItemSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (string.IsNullOrWhiteSpace(query.SearchInput))
                return Enumerable.Empty<ItemSearchReadModel>();

            IEnumerable<StoreItem> storeItems = await itemRepository
                .FindByAsync(query.SearchInput.Trim(), query.StoreId, cancellationToken);

            IEnumerable<ItemCategory> itemCategories = await itemCategoryRepository.FindByAsync(
                storeItems.Select(item => item.ItemCategoryId), cancellationToken);
            IEnumerable<Manufacturer> manufacturers = await manufacturerRepository.FindByAsync(
                storeItems.Select(item => item.ManufacturerId), cancellationToken);

            var itemCategoriesDict = itemCategories.ToDictionary(cat => cat.Id);
            var manufacturersDict = manufacturers.ToDictionary(m => m.Id);

            return storeItems.Select(item =>
                item.ToItemSearchReadModel(
                    query.StoreId,
                    itemCategoriesDict[item.ItemCategoryId],
                    manufacturersDict[item.ManufacturerId]));
        }
    }
}