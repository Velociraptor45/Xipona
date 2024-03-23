using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    public class SearchItemByItemCategoryResultContract
    {
        public SearchItemByItemCategoryResultContract(Guid itemId, Guid? itemTypeId, string name, string manufacturerName,
            IEnumerable<SearchItemByItemCategoryAvailabilityContract> availabilities)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Name = name;
            ManufacturerName = manufacturerName;
            Availabilities = availabilities;
        }

        public Guid ItemId { get; set; }
        public Guid? ItemTypeId { get; set; }
        public string Name { get; set; }
        public string ManufacturerName { get; }
        public IEnumerable<SearchItemByItemCategoryAvailabilityContract> Availabilities { get; set; }
    }
}