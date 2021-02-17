using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults
{
    public class ItemFilterResultsQuery : IQuery<IEnumerable<ItemFilterResultReadModel>>
    {
        public ItemFilterResultsQuery(IEnumerable<ShoppingListStoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
        {
            StoreIds = storeIds;
            ItemCategoriesIds = itemCategoriesIds;
            ManufacturerIds = manufacturerIds;
        }

        public IEnumerable<ShoppingListStoreId> StoreIds { get; }
        public IEnumerable<ItemCategoryId> ItemCategoriesIds { get; }
        public IEnumerable<ManufacturerId> ManufacturerIds { get; }
    }
}