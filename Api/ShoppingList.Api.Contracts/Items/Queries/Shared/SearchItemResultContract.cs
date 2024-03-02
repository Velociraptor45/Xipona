using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared
{
    public class SearchItemResultContract
    {
        public SearchItemResultContract(Guid itemId, string itemName, string manufacturerName)
        {
            ItemId = itemId;
            ItemName = itemName;
            ManufacturerName = manufacturerName;
        }

        public Guid ItemId { get; }
        public string ItemName { get; }
        public string ManufacturerName { get; }
    }
}