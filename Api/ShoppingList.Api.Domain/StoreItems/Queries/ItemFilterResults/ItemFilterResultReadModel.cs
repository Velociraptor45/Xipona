using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemFilterResults
{
    public class ItemFilterResultReadModel
    {
        public ItemFilterResultReadModel(ItemId id, string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
            {
                throw new System.ArgumentException($"'{nameof(itemName)}' cannot be null or empty", nameof(itemName));
            }

            Id = id;
            ItemName = itemName;
        }

        public ItemId Id { get; }
        public string ItemName { get; }
    }
}