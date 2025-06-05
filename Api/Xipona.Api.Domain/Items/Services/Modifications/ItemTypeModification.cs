using Xipona.Api.Domain.Items.Models;

namespace Xipona.Api.Domain.Items.Services.Modifications;

public class ItemTypeModification
{
    public ItemTypeModification(ItemTypeId? id, ItemTypeName name, IEnumerable<ItemAvailability> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId? Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<ItemAvailability> Availabilities { get; }
}