using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

public class StoreUpdate
{
    public StoreUpdate(StoreId id, StoreName name, IEnumerable<SectionUpdate> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToArray();
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public IReadOnlyCollection<SectionUpdate> Sections { get; }
}