using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

public class ItemTypeReadModel
{
    public ItemTypeReadModel(ItemTypeId id, string name, IEnumerable<StoreItemAvailabilityReadModel> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId Id { get; }
    public string Name { get; }
    public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities { get; }
}