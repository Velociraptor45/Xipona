using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class ItemTypeReadModel
{
    public ItemTypeReadModel(ItemTypeId id, ItemTypeName name, IEnumerable<StoreItemAvailabilityReadModel> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities { get; }
}