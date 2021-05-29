using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels
{
    public class ItemSearchReadModelConversionService : IItemSearchReadModelConversionService
    {
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public ItemSearchReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository)
        {
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<ItemSearchReadModel>> ConvertAsync(IEnumerable<IStoreItem> items,
            IStore store, CancellationToken cancellationToken)
        {
            var itemCategoryIds = items
                .Where(i => i.ItemCategoryId != null)
                .Select(i => i.ItemCategoryId)
                .Distinct();
            var itemCategoryDict = (await itemCategoryRepository.FindByAsync(itemCategoryIds, cancellationToken))
                .ToDictionary(i => i.Id);

            var manufacturerIds = items
                .Where(i => i.ManufacturerId != null)
                .Select(i => i.ManufacturerId)
                .Distinct();
            var manufaturerDict = (await manufacturerRepository.FindByAsync(manufacturerIds, cancellationToken))
                .ToDictionary(m => m.Id);

            return items
                .Select(item =>
                {
                    IManufacturer manufacturer = item.ManufacturerId == null ? null : manufaturerDict[item.ManufacturerId];
                    IItemCategory itemCategory = item.ItemCategoryId == null ? null : itemCategoryDict[item.ItemCategoryId];

                    IStoreItemAvailability storeAvailability = item.Availabilities
                        .Single(av => av.StoreId == store.Id);

                    var section = store.Sections.Single(s => s.Id == storeAvailability.DefaultSectionId);

                    return new ItemSearchReadModel(
                        item.Id,
                        item.Name,
                        item.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                        storeAvailability.Price,
                        manufacturer?.ToReadModel(),
                        itemCategory?.ToReadModel(),
                        section.ToReadModel());
                });
        }
    }
}