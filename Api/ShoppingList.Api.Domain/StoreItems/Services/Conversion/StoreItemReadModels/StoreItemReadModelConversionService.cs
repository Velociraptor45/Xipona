using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels
{
    public class StoreItemReadModelConversionService : IStoreItemReadModelConversionService
    {
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IStoreRepository storeRepository;

        public StoreItemReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IStoreRepository storeRepository)
        {
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.storeRepository = storeRepository;
        }

        public async Task<StoreItemReadModel> ConvertAsync(IStoreItem item, CancellationToken cancellationToken)
        {
            if (item is null)
                throw new System.ArgumentNullException(nameof(item));

            IItemCategory? itemCategory = null;
            IManufacturer? manufacturer = null;

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

            return ToReadModel(item, itemCategory, manufacturer, storeDict);
        }

        public StoreItemReadModel ToReadModel(IStoreItem model, IItemCategory? itemCategory,
            IManufacturer? manufacturer, Dictionary<StoreId, IStore> stores)
        {
            var availabilityReadModels = ToAvailabilityReadModel(model.Availabilities, stores).ToList();

            var itemTypeReadModels = new List<ItemTypeReadModel>();
            foreach (var itemType in model.ItemTypes)
            {
                var itemTypeReadModel = new ItemTypeReadModel(itemType.Id, itemType.Name,
                    ToAvailabilityReadModel(itemType.Availabilities, stores));
                itemTypeReadModels.Add(itemTypeReadModel);
            }

            return new StoreItemReadModel(
                model.Id,
                model.Name,
                model.IsDeleted,
                model.Comment,
                model.IsTemporary,
                model.QuantityType.ToReadModel(),
                model.QuantityInPacket,
                model.QuantityTypeInPacket.ToReadModel(),
                itemCategory?.ToReadModel(),
                manufacturer?.ToReadModel(),
                availabilityReadModels,
                itemTypeReadModels);
        }

        private IEnumerable<StoreItemAvailabilityReadModel> ToAvailabilityReadModel(
            IEnumerable<IStoreItemAvailability> availabilities, Dictionary<StoreId, IStore> stores)
        {
            foreach (var av in availabilities)
            {
                var store = stores[av.StoreId];
                var section = store.Sections.First(s => s.Id == av.DefaultSectionId);

                yield return av.ToReadModel(store, section);
            }
        }
    }
}