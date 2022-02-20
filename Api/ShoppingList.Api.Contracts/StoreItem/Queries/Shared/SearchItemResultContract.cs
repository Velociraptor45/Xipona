using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared
{
    public class SearchItemResultContract
    {
        public SearchItemResultContract(int itemId, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                throw new ArgumentException($"'{nameof(itemName)}' cannot be null or whitespace", nameof(itemName));
            }

            ItemId = itemId;
            ItemName = itemName;
        }

        public int ItemId { get; }
        public string ItemName { get; }
    }
}