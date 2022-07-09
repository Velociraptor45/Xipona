using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public class ItemTypeUpdate
{
    public ItemTypeUpdate(ItemTypeId oldId, ItemTypeName name, IEnumerable<IItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name;
        Availabilities = availabilities?.ToList() ?? throw new ArgumentNullException(nameof(availabilities));
    }

    public ItemTypeId OldId { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<IItemAvailability> Availabilities { get; }
}