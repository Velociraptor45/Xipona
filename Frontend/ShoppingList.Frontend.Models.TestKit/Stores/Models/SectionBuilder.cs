using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using ShoppingList.Frontend.TestTools;

namespace ShoppingList.Frontend.Models.TestKit.Stores.Models;

public class SectionBuilder : TestBuilderBase<Section>
{
    public SectionBuilder WithIsDefaultSection(bool isDefaultSection)
    {
        FillConstructorWith(nameof(isDefaultSection), isDefaultSection);
        return this;
    }
}