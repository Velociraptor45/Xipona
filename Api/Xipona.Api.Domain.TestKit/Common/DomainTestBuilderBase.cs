using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Common;

public abstract class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    protected DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}