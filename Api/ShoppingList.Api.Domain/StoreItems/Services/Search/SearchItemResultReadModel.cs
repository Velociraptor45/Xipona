using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;

public class SearchItemResultReadModel
{
    public SearchItemResultReadModel(ItemId id, string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            throw new ArgumentException($"'{nameof(itemName)}' cannot be null or empty", nameof(itemName));
        }

        Id = id;
        ItemName = itemName;
    }

    public ItemId Id { get; }
    public string ItemName { get; }
}