using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;

public static class ItemStoreMother
{
    public static ItemStore GetValid()
    {
        return new DomainTestBuilder<ItemStore>().Create() with
        {
            Sections = CreateSections().ToList()
        };
    }

    private static IEnumerable<ItemStoreSection> CreateSections()
    {
        for (int i = 0; i < 2; i++)
        {
            var section = new DomainTestBuilder<ItemStoreSection>().Create() with
            {
                IsDefaultSection = i == 0,
                SortingIndex = i
            };
            yield return section;
        }
    }
}