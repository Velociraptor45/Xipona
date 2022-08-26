using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Stores.Entities;

public class StoreEntityBuilder : TestBuilder<Store>
{
    public StoreEntityBuilder()
    {
        WithSections(new SectionEntityBuilder().CreateMany(3));
    }

    public StoreEntityBuilder WithSections(IEnumerable<Section> sections)
    {
        FillPropertyWith(s => s.Sections, sections.ToList());
        return this;
    }

    public StoreEntityBuilder WithDeleted(bool deleted)
    {
        FillPropertyWith(s => s.Deleted, deleted);
        return this;
    }
}