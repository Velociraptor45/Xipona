using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Queries.ItemFilterResults
{
    public class ItemFilterResultReadModel
    {
        public ItemFilterResultReadModel(StoreItemActualId id, string ItemName)
        {
            if (string.IsNullOrEmpty(ItemName))
            {
                throw new System.ArgumentException($"'{nameof(ItemName)}' cannot be null or empty", nameof(ItemName));
            }

            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            this.ItemName = ItemName;
        }

        public StoreItemActualId Id { get; }
        public string ItemName { get; }
    }
}