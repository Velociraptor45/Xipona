using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

public class ItemTypeModification
{
    public ItemTypeModification(ItemTypeId? id, ItemTypeName name, IEnumerable<IStoreItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId? Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
}