﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores
{
    public class AllActiveStoresQueryHandler : IQueryHandler<AllActiveStoresQuery, IEnumerable<ActiveStoreReadModel>>
    {
        private readonly IStoreRepository storeRepository;
        private readonly IItemRepository itemRepository;

        public AllActiveStoresQueryHandler(IStoreRepository storeRepository, IItemRepository itemRepository)
        {
            this.storeRepository = storeRepository;
            this.itemRepository = itemRepository;
        }

        public async Task<IEnumerable<ActiveStoreReadModel>> HandleAsync(AllActiveStoresQuery query, CancellationToken cancellationToken)
        {
            var activeStores = (await storeRepository.FindActiveStoresAsync(cancellationToken)).ToList();
            var itemPerStoreDict = new Dictionary<StoreId, IEnumerable<StoreItem>>();

            foreach (var store in activeStores)
            {
                var items = await itemRepository.FindByAsync(store.Id, cancellationToken);
                itemPerStoreDict.Add(store.Id, items);
            }

            cancellationToken.ThrowIfCancellationRequested();

            return activeStores.Select(store => store.ToActiveStoreReadModel(itemPerStoreDict[store.Id]));
        }
    }
}