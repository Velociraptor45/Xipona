using ShoppingList.Api.Core.TestKit;

namespace ShoppingList.Api.Domain.TestKit.Common;

public abstract class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    protected DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}