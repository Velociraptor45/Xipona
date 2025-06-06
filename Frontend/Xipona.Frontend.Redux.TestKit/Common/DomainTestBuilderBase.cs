using Xipona.Frontend.TestTools;

namespace Xipona.Frontend.Redux.TestKit.Common;

public class DomainTestBuilderBase<TModel> : TestBuilderBase<TModel>
{
    public DomainTestBuilderBase() : base(new DomainCustomization())
    {
    }
}