using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

public class StoreUpdate
{
    public StoreUpdate(StoreId id, StoreName name, IEnumerable<SectionUpdate> sections)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Sections = sections?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(sections));
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public IReadOnlyCollection<SectionUpdate> Sections { get; }
}