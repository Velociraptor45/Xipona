using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    public class SearchItemByItemCategoryResultContract
    {
        public SearchItemByItemCategoryResultContract(Guid itemId, Guid? itemTypeId, string name,
            IEnumerable<SearchItemByItemCategoryAvailabilityContract> availabilities)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Name = name;
            Availabilities = availabilities;
        }

        public Guid ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
        public string Name { get; set; }
        public IEnumerable<SearchItemByItemCategoryAvailabilityContract> Availabilities { get; set; }
    }
}