using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;

public class ItemTypeModification
{
    public ItemTypeModification(ItemTypeId? id, ItemTypeName name, IEnumerable<IItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId? Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<IItemAvailability> Availabilities { get; }
}