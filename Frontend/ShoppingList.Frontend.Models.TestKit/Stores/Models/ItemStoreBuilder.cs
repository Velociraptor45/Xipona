using ProjectHermes.ShoppingList.Frontend.TestTools;
using ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;

public class ItemStoreBuilder : TestBuilderBase<ItemStore>
{
    public ItemStoreBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public ItemStoreBuilder WithSections(IEnumerable<ItemStoreSection> sections)
    {
        FillConstructorWith(nameof(sections), sections);
        return this;
    }

    public ItemStoreBuilder WithoutSections()
    {
        return WithSections(Enumerable.Empty<ItemStoreSection>());
    }
}