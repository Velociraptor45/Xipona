using System;

namespace ShoppingList.Api.Contracts.Queries.ItemFilterResults
{
    public class ItemFilterResultContract
    {
        public ItemFilterResultContract(int itemId, string itemName)
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