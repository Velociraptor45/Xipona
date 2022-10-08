using ProjectHermes.ShoppingList.Api.Core.TestKit;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

public class DomainTestBuilder<TModel> : TestBuilder<TModel, DomainTestBuilder<TModel>>
{
    public DomainTestBuilder() : base(new DomainCustomization())
    {
    }
}