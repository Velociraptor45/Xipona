using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

public class ItemTypeUpdate
{
    public ItemTypeUpdate(ItemTypeId oldId, ItemTypeName name, IEnumerable<IStoreItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name;
        Availabilities = availabilities?.ToList() ?? throw new ArgumentNullException(nameof(availabilities));
    }

    public ItemTypeId OldId { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
}