using ShoppingList.Api.Core.TestKit;

namespace ShoppingList.Api.Domain.TestKit.Common;

public class DomainTestBuilder<TModel> : TestBuilder<TModel, DomainTestBuilder<TModel>>
{
    public DomainTestBuilder() : base(new DomainCustomization())
    {
    }
}