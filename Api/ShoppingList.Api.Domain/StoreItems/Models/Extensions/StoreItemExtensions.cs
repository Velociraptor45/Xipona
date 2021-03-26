using ProjectHermes.ShoppingList.Api.Core.Attributes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearch;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemExtensions
    {
        public static ItemSearchReadModel ToItemSearchReadModel(this IStoreItem storeItem, StoreId storeId,
            IItemCategory itemCategory, IManufacturer manufacturer, IStoreSection defaultSection,
            IStoreItemAvailability storeAvailability)
        {
            return new ItemSearchReadModel(
                storeItem.Id,
                storeItem.Name,
                storeItem.QuantityType.GetAttribute<DefaultQuantityAttribute>().DefaultQuantity,
                storeAvailability.Price,
                manufacturer?.ToReadModel(),
                itemCategory?.ToReadModel(),
                defaultSection.ToReadModel());
        }

        public static ItemFilterResultReadModel ToItemFilterResultReadModel(this IStoreItem model)
        {
            return new ItemFilterResultReadModel(model.Id, model.Name);
        }

        public static StoreItemReadModel ToReadModel(this IStoreItem model, IItemCategory itemCategory,
            IManufacturer manufacturer, Dictionary<StoreId, IStore> stores)
        {
            var availabilityReadModels = new List<StoreItemAvailabilityReadModel>();
            foreach (var av in model.Availabilities)
            {
                var store = stores[av.StoreId];
                var section = store.Sections.First(s => s.Id == av.DefaultSectionId);

                availabilityReadModels.Add(av.ToReadModel(store, section));
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
                availabilityReadModels);
        }
    }
}