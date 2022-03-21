using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Store : IStore
{
    private readonly StoreSections _sections;

    public Store(StoreId id, StoreName name, bool isDeleted, StoreSections sections)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsDeleted = isDeleted;
        _sections = sections;
    }

    public StoreId Id { get; }
    public StoreName Name { get; private set; }
    public bool IsDeleted { get; }
    public IReadOnlyCollection<IStoreSection> Sections => _sections.AsReadOnly();

    public IStoreSection GetDefaultSection()
    {
        return _sections.GetDefaultSection();
    }

    public bool ContainsSection(SectionId sectionId)
    {
        return _sections.Contains(sectionId);
    }

    public void ChangeName(StoreName name)
    {
        ArgumentNullException.ThrowIfNull(name);

        Name = name;
    }

    public void UpdateSections(IEnumerable<SectionUpdate> sectionUpdates)
    {
        _sections.UpdateMany(sectionUpdates);
    }
}