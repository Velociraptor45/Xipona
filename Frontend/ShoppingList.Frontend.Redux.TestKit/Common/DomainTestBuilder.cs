using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class DomainTestBuilder<TModel> : TestBuilder<TModel, DomainTestBuilder<TModel>>
{
    public DomainTestBuilder() : base(new DomainCustomization())
    {
    }
}