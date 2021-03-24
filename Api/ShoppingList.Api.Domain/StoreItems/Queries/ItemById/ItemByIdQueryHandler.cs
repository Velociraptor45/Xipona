using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemById
{
    public class ItemByIdQueryHandler : IQueryHandler<ItemByIdQuery, StoreItemReadModel>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IStoreRepository storeRepository;

        public ItemByIdQueryHandler(IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository,
            IStoreRepository storeRepository)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<StoreItemReadModel> HandleAsync(ItemByIdQuery query, CancellationToken cancellationToken)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            cancellationToken.ThrowIfCancellationRequested();

            var item = await itemRepository.FindByAsync(query.ItemId, cancellationToken);
            if (item == null)
                throw new DomainException(new ItemNotFoundReason(query.ItemId));

            cancellationToken.ThrowIfCancellationRequested();

            IItemCategory itemCategory = null;
            IManufacturer manufacturer = null;

            if (item.ItemCategoryId != null)
            {
                itemCategory = await itemCategoryRepository.FindByAsync(item.ItemCategoryId, cancellationToken);
                if (itemCategory == null)
                    throw new DomainException(new ItemCategoryNotFoundReason(item.ItemCategoryId));
            }
            if (item.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository.FindByAsync(item.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new DomainException(new ManufacturerNotFoundReason(item.ManufacturerId));
            }

            var storeIds = item.Availabilities.Select(av => av.StoreId);
            var storeDict = (await storeRepository.FindByAsync(storeIds, cancellationToken))
                .ToDictionary(store => store.Id);

            return item.ToReadModel(itemCategory, manufacturer, storeDict);
        }
    }
}