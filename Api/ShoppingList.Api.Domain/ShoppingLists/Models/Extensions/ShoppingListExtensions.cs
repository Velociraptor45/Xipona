using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Extensions
{
    public static class ShoppingListExtensions
    {
        public static ShoppingListReadModel ToReadModel(this IShoppingList model, IStore store,
            Dictionary<ItemId, IStoreItem> storeItems)
        {
            List<ShoppingListSectionReadModel> sectionReadModels = new List<ShoppingListSectionReadModel>();
            foreach (var section in model.Sections)
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
                        storeItem.Availabilities.First(av => av.StoreId.Id == store.Id).Price,
                        storeItem.QuantityType.ToReadModel(),
                        storeItem.QuantityInPacket,
                        storeItem.QuantityTypeInPacket.ToReadModel(),
                        storeItem.ItemCategoryId?.ToReadModel(),
                        storeItem.ManufacturerId?.ToReadModel(),
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
                model.Id,
                model.CompletionDate,
                store.ToShoppingListStoreReadModel(),
                sectionReadModels);
        }
    }
}