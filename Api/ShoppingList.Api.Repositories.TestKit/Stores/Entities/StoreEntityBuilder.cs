using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Stores.Entities;

public class StoreEntityBuilder : TestBuilderBase<Store>
{
    public StoreEntityBuilder()
    {
        WithSections(new SectionEntityBuilder().CreateMany(3).ToList());
    }

    public StoreEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public StoreEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public StoreEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(p => p.Deleted, deleted);
        return this;
    }

    public StoreEntityBuilder WithSections(ICollection<Section> sections)
    {
        FillPropertyWith(p => p.Sections, sections);
        return this;
    }
}