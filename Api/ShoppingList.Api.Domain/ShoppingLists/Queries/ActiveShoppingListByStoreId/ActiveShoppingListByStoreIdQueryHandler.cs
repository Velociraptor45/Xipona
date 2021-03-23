using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ActiveShoppingListByStoreIdQueryHandler
        : IQueryHandler<ActiveShoppingListByStoreIdQuery, ShoppingListReadModel>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ActiveShoppingListByStoreIdQueryHandler(IShoppingListRepository shoppingListRepository,
            IStoreRepository storeRepository, IItemRepository itemRepository, 
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.storeRepository = storeRepository;
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<ShoppingListReadModel> HandleAsync(ActiveShoppingListByStoreIdQuery query,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var shoppingList = await shoppingListRepository.FindActiveByAsync(query.StoreId, cancellationToken);
            if (shoppingList == null)
                throw new DomainException(new ShoppingListNotFoundReason(query.StoreId));

            var ItemIds = shoppingList.Items.Select(i => i.Id);
            var itemsDict = (await itemRepository.FindByAsync(ItemIds, cancellationToken))
                .ToDictionary(i => i.Id);

            var itemCategoryIds = itemsDict.Values.Where(i => i.ItemCategoryId != null).Select(i => i.ItemCategoryId);
            var itemCategoriesDict = (await itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
                .ToDictionary(cat => cat.Id);
                        
            var manufacturerIds = itemsDict.Values.Where(i => i.ManufacturerId != null).Select(i => i.ManufacturerId);
            var manufacturersDict = (await manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
                .ToDictionary(man => man.Id);

            IStore store = await storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);

            return shoppingList.ToReadModel(store, itemsDict, itemCategoriesDict, manufacturersDict);
        }
    }
}