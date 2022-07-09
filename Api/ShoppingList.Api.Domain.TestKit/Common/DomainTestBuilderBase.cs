using ProjectHermes.ShoppingList.Api.Core.TestKit;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

public abstract class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    protected DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}