using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Conversion.ShoppingListReadModels
{
    public class ShoppingListReadModelConversionService : IShoppingListReadModelConversionService
    {
        private readonly IStoreRepository storeRepository;
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ShoppingListReadModelConversionService(IStoreRepository storeRepository, IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository, IManufacturerRepository manufacturerRepository)
        {
            this.storeRepository = storeRepository;
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<ShoppingListReadModel> ConvertAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            if (shoppingList is null)
                throw new System.ArgumentNullException(nameof(shoppingList));

            var ItemIds = shoppingList.Items.Select(i => i.Id);
            var itemsDict = (await itemRepository.FindByAsync(ItemIds, cancellationToken))
                .ToDictionary(i => i.Id);

            var itemCategoryIds = itemsDict.Values.Where(i => i.ItemCategoryId != null).Select(i => i.ItemCategoryId!);
            var itemCategoriesDict = (await itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
                .ToDictionary(cat => cat.Id);

            var manufacturerIds = itemsDict.Values.Where(i => i.ManufacturerId != null).Select(i => i.ManufacturerId!);
            var manufacturersDict = (await manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
                .ToDictionary(man => man.Id);

            IStore? store = await storeRepository.FindByAsync(shoppingList.StoreId, cancellationToken);
            if (store is null)
                throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

            return ToReadModel(shoppingList, store, itemsDict, itemCategoriesDict, manufacturersDict);
        }

        private ShoppingListReadModel ToReadModel(IShoppingList shoppingList, IStore store,
            Dictionary<ItemId, IStoreItem> storeItems, Dictionary<ItemCategoryId, IItemCategory> itemCategories,
            Dictionary<ManufacturerId, IManufacturer> manufacturers)
        {
            List<ShoppingListSectionReadModel> sectionReadModels = new List<ShoppingListSectionReadModel>();
            foreach (var section in shoppingList.Sections)
            {
                List<ShoppingListItemReadModel> itemReadModels = new List<ShoppingListItemReadModel>();
                foreach (var item in section.Items)
                {
                    var storeItem = storeItems[item.Id];

                    var itemReadModel = new ShoppingListItemReadModel(
                        item.Id,
                        storeItem.Name,
                        storeItem.IsDeleted,
                        storeItem.Comment,
                        storeItem.IsTemporary,
                        storeItem.Availabilities.First(av => av.StoreId == store.Id).Price,
                        storeItem.QuantityType.ToReadModel(),
                        storeItem.QuantityInPacket,
                        storeItem.QuantityTypeInPacket.ToReadModel(),
                        storeItem.ItemCategoryId == null ? null : itemCategories[storeItem.ItemCategoryId].ToReadModel(),
                        storeItem.ManufacturerId == null ? null : manufacturers[storeItem.ManufacturerId].ToReadModel(),
                        item.IsInBasket,
                        item.Quantity);

                    itemReadModels.Add(itemReadModel);
                }

                var storeSection = store.Sections.First(s => s.Id == section.Id);

                var sectionReadModel = new ShoppingListSectionReadModel(
                    section.Id,
                    storeSection.Name,
                    storeSection.SortingIndex,
                    storeSection.IsDefaultSection,
                    itemReadModels);

                sectionReadModels.Add(sectionReadModel);
            }

            return new ShoppingListReadModel(
                shoppingList.Id,
                shoppingList.CompletionDate,
                store.ToShoppingListStoreReadModel(),
                sectionReadModels);
        }
    }
}