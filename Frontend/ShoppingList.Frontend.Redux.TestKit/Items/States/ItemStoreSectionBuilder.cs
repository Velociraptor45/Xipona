using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;
public class ItemStoreSectionBuilder : TestBuilderBase<ItemStoreSection>
{
    public ItemStoreSectionBuilder WithId(Guid id)
    {
        FillConstructorWith("Id", id);
        return this;
    }

    public ItemStoreSectionBuilder WithName(string name)
    {
        FillConstructorWith("Name", name);
        return this;
    }

    public ItemStoreSectionBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith("IsDefaultSection", isDefaultSection);
        return this;
    }

    public ItemStoreSectionBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith("SortingIndex", sortingIndex);
        return this;
    }
}