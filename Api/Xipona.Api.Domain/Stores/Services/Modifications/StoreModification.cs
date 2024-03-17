using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Services.Modifications;

public class StoreModification
{
    public StoreModification(StoreId id, StoreName name, IEnumerable<SectionModification> sections)
    {
        Id = id;
        Name = name;
        Sections = sections.ToArray();
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
    public IReadOnlyCollection<SectionModification> Sections { get; }
}