using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;

public class SearchItemResultReadModel
{
    public SearchItemResultReadModel(ItemId id, ItemName itemName)
    {
        Id = id;
        ItemName = itemName;
    }

    public SearchItemResultReadModel(IStoreItem item) :
        this(item.Id, item.Name)
    {
    }

    public ItemId Id { get; }
    public ItemName ItemName { get; }
}