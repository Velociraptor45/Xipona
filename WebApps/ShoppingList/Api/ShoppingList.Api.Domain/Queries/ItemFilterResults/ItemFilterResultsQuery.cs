using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Queries.ItemFilterResults
{
    public class ItemFilterResultsQuery : IQuery<IEnumerable<ItemFilterResultReadModel>>
    {
        public ItemFilterResultsQuery(IEnumerable<StoreId> storeIds,
            IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
        {
            StoreIds = storeIds;
            ItemCategoriesIds = itemCategoriesIds;
            ManufacturerIds = manufacturerIds;
        }

        public IEnumerable<StoreId> StoreIds { get; }
        public IEnumerable<ItemCategoryId> ItemCategoriesIds { get; }
        public IEnumerable<ManufacturerId> ManufacturerIds { get; }
    }
}