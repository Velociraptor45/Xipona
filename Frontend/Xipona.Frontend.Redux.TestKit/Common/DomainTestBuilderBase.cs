using ProjectHermes.Xipona.Frontend.TestTools;

namespace ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}