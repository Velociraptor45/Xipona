using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults
{
    public class ItemFilterResultReadModel
    {
        public ItemFilterResultReadModel(ItemId id, string ItemName)
        {
            if (string.IsNullOrEmpty(ItemName))
            {
                throw new System.ArgumentException($"'{nameof(ItemName)}' cannot be null or empty", nameof(ItemName));
            }

            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            this.ItemName = ItemName;
        }

        public ItemId Id { get; }
        public string ItemName { get; }
    }
}