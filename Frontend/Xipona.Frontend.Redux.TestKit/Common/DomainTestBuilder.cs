using ProjectHermes.Xipona.Frontend.TestTools;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

public class DomainTestBuilder<TModel> : TestBuilder<TModel, DomainTestBuilder<TModel>>
{
    public DomainTestBuilder() : base(new DomainCustomization())
    {
    }
}