using ProjectHermes.Xipona.Api.Domain.Items.Models;

namespace ProjectHermes.Xipona.Api.Domain.Items.Services.Queries;

public class ItemTypeReadModel
{
    public ItemTypeReadModel(ItemTypeId id, ItemTypeName name, IEnumerable<ItemAvailabilityReadModel> availabilities)
    {
        Id = id;
        Name = name;
        Availabilities = availabilities.ToList();
    }

    public ItemTypeId Id { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<ItemAvailabilityReadModel> Availabilities { get; }
}