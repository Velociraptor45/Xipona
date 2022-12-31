using ProjectHermes.ShoppingList.Frontend.TestTools;
using ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;

public class ItemStoreSectionBuilder : TestBuilderBase<ItemStoreSection>
{
    public ItemStoreSectionBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith(nameof(isDefaultSection), isDefaultSection);
        return this;
    }

    public ItemStoreSectionBuilder WithSortingIndex(int sortingIndex)
    {
        FillConstructorWith(nameof(sortingIndex), sortingIndex);
        return this;
    }
}