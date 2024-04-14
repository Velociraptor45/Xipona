using ProjectHermes.Xipona.Api.Core.TestKit;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Common;

public class DomainTestBuilder<TModel> : TestBuilder<TModel, DomainTestBuilder<TModel>>
{
    public DomainTestBuilder() : base(new DomainCustomization())
    {
    }
}