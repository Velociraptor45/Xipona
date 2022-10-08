using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Models.TestKit.Stores.Models;

public class SectionBuilder : TestBuilderBase<Section>
{
    public SectionBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith(nameof(isDefaultSection), isDefaultSection);
        return this;
    }
}