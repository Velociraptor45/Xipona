using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;

public class StoreFactory : IStoreFactory
{
    private readonly ISectionFactory _sectionFactory;

    public StoreFactory(ISectionFactory sectionFactory)
    {
        _sectionFactory = sectionFactory;
    }

    public IStore Create(StoreId id, StoreName name, bool isDeleted, IEnumerable<ISection> sections)
    {
        return new Store(id, name, isDeleted, new Sections(sections, _sectionFactory));
    }

    public IStore CreateNew(StoreCreation creationInfo)
    {
        var sections = creationInfo.Sections.Select(s => _sectionFactory.CreateNew(s));

        return Create(new StoreId(Guid.NewGuid()), creationInfo.Name, false, sections);
    }
}