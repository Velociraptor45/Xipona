using ProjectHermes.ShoppingList.Frontend.TestTools;

namespace ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}