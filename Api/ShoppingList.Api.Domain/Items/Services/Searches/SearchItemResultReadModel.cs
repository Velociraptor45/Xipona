using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public class SearchItemResultReadModel
{
    public SearchItemResultReadModel(ItemId id, ItemName itemName)
    {
        Id = id;
        ItemName = itemName;
    }

    public SearchItemResultReadModel(IItem item) :
        this(item.Id, item.Name)
    {
    }

    public ItemId Id { get; }
    public ItemName ItemName { get; }
}