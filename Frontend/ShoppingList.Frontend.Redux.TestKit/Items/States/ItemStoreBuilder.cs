using ProjectHermes.ShoppingList.Frontend.TestTools;
using ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;
public class ItemStoreBuilder : TestBuilderBase<ItemStore>
{
    public ItemStoreBuilder WithId(Guid id)
    {
        FillConstructorWith("Id", id);
        return this;
    }

    public ItemStoreBuilder WithName(string name)
    {
        FillConstructorWith("Name", name);
        return this;
    }

    public ItemStoreBuilder WithSections(IReadOnlyCollection<ItemStoreSection> sections)
    {
        FillConstructorWith("Sections", sections);
        return this;
    }
}