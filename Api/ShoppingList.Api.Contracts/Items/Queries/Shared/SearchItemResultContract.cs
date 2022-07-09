using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared
{
    public class SearchItemResultContract
    {
        public SearchItemResultContract(Guid itemId, string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                throw new ArgumentException($"'{nameof(itemName)}' cannot be null or whitespace", nameof(itemName));
            }

            ItemId = itemId;
            ItemName = itemName;
        }

        public Guid ItemId { get; }
        public string ItemName { get; }
    }
}