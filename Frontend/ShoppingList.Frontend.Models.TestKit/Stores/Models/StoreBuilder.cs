using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ShoppingList.Frontend.TestTools;

namespace ShoppingList.Frontend.Models.TestKit.Stores.Models;

public class StoreBuilder : TestBuilderBase<Store>
{
    public StoreBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public StoreBuilder WithSections(IEnumerable<Section> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public StoreBuilder WithoutSections()
    {
        return WithSections(Enumerable.Empty<Section>());
    }
}