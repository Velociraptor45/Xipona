﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults
{
    public class ItemFilterResultsQueryHandler : IQueryHandler<ItemFilterResultsQuery, IEnumerable<ItemFilterResultReadModel>>
    {
        private readonly IItemRepository itemRepository;

        public ItemFilterResultsQueryHandler(IItemRepository itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ItemFilterResultReadModel>> HandleAsync(
            ItemFilterResultsQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new System.ArgumentNullException(nameof(query));
            }

            var storeItems = await itemRepository.FindByAsync(query.StoreIds, query.ItemCategoriesIds, query.ManufacturerIds,
                cancellationToken);

            return storeItems
                .Where(model => !model.IsDeleted)
                .Select(model => model.ToItemFilterResultReadModel());
        }
    }
}