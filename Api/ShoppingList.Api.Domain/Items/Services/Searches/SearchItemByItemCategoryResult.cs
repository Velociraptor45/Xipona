using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public class SearchItemByItemCategoryResult
{
    public SearchItemByItemCategoryResult(ItemId id, ItemTypeId? itemTypeId, string name,
        IEnumerable<ItemAvailabilityReadModel> availabilities)
    {
        Id = id;
        ItemTypeId = itemTypeId;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemId Id { get; }
    public ItemTypeId? ItemTypeId { get; }
    public string Name { get; }
    public IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities { get; }
}